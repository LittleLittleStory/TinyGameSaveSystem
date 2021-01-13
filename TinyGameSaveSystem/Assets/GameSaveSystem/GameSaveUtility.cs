using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSaveUtility
{
    private static string[] assemblyNames = { "Assembly-CSharp" };
    private static GameSaveSystem gameSaveSystem
    {
        get
        {
            return SingletonManager.GetSingleton<GameSaveSystem>();
        }
    }
    public static SceneData AddSceneData(string sceneName)
    {
        if (gameSaveSystem.GameData.SceneDatas.ContainsKey(sceneName))
        {
            Debug.LogError(string.Format("存档已存在{0}对应的场景", sceneName));
            return null;
        }
        else
        {
            SceneData sceneData = new SceneData(sceneName);
            gameSaveSystem.GameData.SceneDatas.Add(sceneName, sceneData);
            return sceneData;
        }
    }

    public static SaveObject AddSaveObject(string name, string sceneName)
    {
        SceneData sceneData = GetSceneData(sceneName);
        if (null == sceneData)
            AddSceneData(sceneName);
        if (sceneData.SaveObjects.ContainsKey(name))
        {
            Debug.LogError(string.Format("存档已存在{0}对应的物体", name));
            return null;
        }
        else
        {
            SaveObject saveObject = new SaveObject(name);
            sceneData.SaveObjects.Add(name, saveObject);
            return saveObject;
        }
    }

    public static GobalData AddGobalObject(string name)
    {
        if (gameSaveSystem.GameData.GobalDatas.ContainsKey(name))
        {
            Debug.LogError(string.Format("存档已存在{0}对应的物体", name));
            return null;
        }
        else
        {
            GobalData gobalData = new GobalData(name);
            gameSaveSystem.GameData.GobalDatas.Add(name, gobalData);
            return gobalData;
        }
    }

    public static void SaveGame()
    {
        ToolUtility.SaveJson(gameSaveSystem.GameData, "GameData");
    }

    public static SceneData GetSceneData(string sceneName)
    {
        if (gameSaveSystem.GameData.SceneDatas.ContainsKey(sceneName))
        {
            return gameSaveSystem.GameData.SceneDatas[sceneName];
        }
        else
        {
            return null;
        }
    }

    public static SaveObject GetSaveObjectData(string name, string sceneName = "")
    {
        sceneName = string.IsNullOrEmpty(sceneName) == true ? SceneManager.GetActiveScene().name : sceneName;
        SceneData sceneData = GetSceneData(sceneName);
        if (null == sceneData)
        {
            Debug.LogError(string.Format("存档未找到{0}对应的场景", sceneName));
            return null;
        }
        if (sceneData.SaveObjects.ContainsKey(name))
            return sceneData.SaveObjects[name];
        else
            return null;
    }

    public static GobalData GetGobalSaveObjectData(string name, string sceneName = "")
    {
        if (gameSaveSystem.GameData.GobalDatas.ContainsKey(name))
            return gameSaveSystem.GameData.GobalDatas[name];
        else
            return null;
    }

    /// <summary>
    /// 保存场景对象组件，只写进内存中的存档
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="gameObject">gameObject</param>
    /// <param name="sceneName">场景名，不填为当前场景</param>
    public static void Save<T1, T2>(this GameObject gameObject, string sceneName = "")
        where T1 : ISave<T2>
    {
        if (gameObject.CheckEmpty())
            return;
        T2 component = gameObject.GetComponent<T2>();
        if (component.CheckEmpty())
            return;

        sceneName = string.IsNullOrEmpty(sceneName) == true ? SceneManager.GetActiveScene().name : sceneName;

        string ISaveName = typeof(T1).Name;
        T1 ISave = (T1)ToolUtility.CreateHelperInstance(ISaveName, assemblyNames);
        string value;
        try
        {
            value = ISave.Save(component);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return;
        }
        GameSaveSystem.Save<T1, T2>(gameObject.name, value, sceneName);
    }

    /// <summary>
    /// 保存全局数据，只写进内存中的存档
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="component"></param>
    public static void Save<T1, T2>(this T2 component)
        where T1 : ISave<T2>
    {
        if (component.CheckEmpty())
            return;
        string ISaveName = typeof(T1).Name;
        T1 ISave = (T1)ToolUtility.CreateHelperInstance(ISaveName, assemblyNames);
        string value;
        try
        {
            value = ISave.Save(component);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return;
        }
        GameSaveSystem.Save<T1, T2>(typeof(T2).Name, value);
    }

    /// <summary>
    ///  读取内存中的存档值
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。
    /// <typeparam name="T2">你希望保存组件对象类型</typeparam>
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="gameObject">gameObject</param>
    /// <param name="sceneName">场景名，不填为当前场景</param>
    /// <returns></returns>
    public static bool Load<T1, T2>(this GameObject gameObject, string sceneName = "")
        where T1 : ISave<T2>
        where T2 : Component
    {
        string ISaveName = typeof(T1).Name;
        string componentName = typeof(T2).Name;
        if (gameObject.CheckEmpty())
            return false;
        T2 component = gameObject.GetComponent<T2>();
        if (component.CheckEmpty())
            return false;

        SaveObject saveObject = GetSaveObjectData(gameObject.name, sceneName);
        if (null == saveObject)
        {
            Debug.LogError(string.Format("存档未找到{0}对应的物体", gameObject.name));
            return false;
        }
        if (false == saveObject.SetValues.ContainsKey(componentName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的组件类型", componentName));
            return false;
        }
        Dictionary<string, SetValue> setValues = saveObject.SetValues[componentName];
        if (false == setValues.ContainsKey(ISaveName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的赋值操作类型", ISaveName));
            return false;
        }
        SetValue setValue = setValues[ISaveName];
        T1 ISave = (T1)ToolUtility.CreateHelperInstance(ISaveName, assemblyNames);
        ISave.Load(component, setValue.Value);
        return true;
    }

    public static bool Load<T1, T2>(this T2 component, string sceneName = "")
        where T1 : ISave<T2>
    {
        string ISaveName = typeof(T1).Name;
        string componentName = typeof(T2).Name;

        GobalData gobalData = GetGobalSaveObjectData(componentName, sceneName);
        if (null == gobalData)
        {
            Debug.LogError(string.Format("存档未找到{0}全局数据", componentName));
            return false;
        }
        if (false == gobalData.SetValues.ContainsKey(componentName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的组件类型", componentName));
            return false;
        }
        Dictionary<string, SetValue> setValues = gobalData.SetValues[componentName];
        if (false == setValues.ContainsKey(ISaveName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的赋值操作类型", ISaveName));
            return false;
        }
        SetValue setValue = setValues[ISaveName];
        T1 ISave = (T1)ToolUtility.CreateHelperInstance(ISaveName, assemblyNames);
        ISave.Load(component, setValue.Value);
        return true;
    }
}
