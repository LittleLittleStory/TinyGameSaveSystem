using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSaveUtility
{
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

    public static SaveObject AddSaveObjectData(string name, string sceneName)
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

    private static SetValue GetComponentSetValue<T1, T2>(GameObject gameObject, SaveObject saveObject)
    where T1 : ISave<T2>
    {
        string ISaveName = typeof(T1).Name;
        string componentName = typeof(T2).Name;
        if (null == saveObject)
        {
            Debug.LogError(string.Format("存档未找到{0}对应的物体", gameObject.name));
            return null;
        }
        if (false == saveObject.SetValues.ContainsKey(componentName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的组件类型", componentName));
            return null;
        }
        Dictionary<string, SetValue> setValues = saveObject.SetValues[componentName];
        if (false == setValues.ContainsKey(ISaveName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的赋值操作类型", ISaveName));
            return null;
        }
        return setValues[ISaveName];
    }

    public static GobalData AddGobalObjectData(string name)
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

    public static GobalData GetGobalObjectData(string name)
    {
        if (gameSaveSystem.GameData.GobalDatas.ContainsKey(name))
            return gameSaveSystem.GameData.GobalDatas[name];
        else
            return null;
    }

    private static SetValue GetGobalSetValue<T1, T2>(GobalData gobalData)
    where T1 : ISave<T2>
    {
        string ISaveName = typeof(T1).Name;
        string componentName = typeof(T2).Name;
        if (null == gobalData)
        {
            Debug.LogError(string.Format("存档未找到{0}全局数据", componentName));
            return null;
        }
        if (false == gobalData.SetValues.ContainsKey(componentName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的组件类型", componentName));
            return null;
        }
        Dictionary<string, SetValue> setValues = gobalData.SetValues[componentName];
        if (false == setValues.ContainsKey(ISaveName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的赋值操作类型", ISaveName));
            return null;
        }
        return setValues[ISaveName];
    }


    /// <summary>
    /// 保存游戏数据
    /// </summary>
    public static void SaveGame()
    {
        ToolUtility.SaveJson(gameSaveSystem.GameData, "GameData");
    }

    /// <summary>
    /// 保存场景对象组件，只写进内存中的存档
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。</typeparam>
    /// <typeparam name="T2">你希望保存组件对象类型
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="gameObject">gameObject</param>
    /// <param name="sceneName">场景名，不填为当前场景</param>
    public static void SaveComponent<T1, T2>(this GameObject gameObject, string sceneName = "")
        where T1 : ISave<T2>
    {
        if (gameObject.CheckEmpty())
            return;
        T2 component = gameObject.GetComponent<T2>();
        if (component.CheckEmpty())
            return;
        GameSaveSystem.SaveComponent<T1, T2>(gameObject, component, sceneName);
    }

    public static void SaveComponent<T1, T2>(this T2 component, string sceneName = "")
        where T1 : ISave<T2>
        where T2 : Component
    {
        if (component.CheckEmpty())
            return;
        GameSaveSystem.SaveComponent<T1, T2>(component.gameObject, component, sceneName);
    }

    /// <summary>
    /// 保存全局数据，只读当前内存中的存档
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。</typeparam>
    /// <typeparam name="T2">你希望保存组件对象类型
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="component">全局对象</param>
    public static void SaveGobal<T1, T2>(this T2 component)
        where T1 : ISave<T2>
    {
        if (component.CheckEmpty())
            return;
        GameSaveSystem.SaveGobal<T1, T2>(component);
    }

    /// <summary>
    ///  读取内存中的存档值
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。</typeparam>
    /// <typeparam name="T2">你希望保存组件对象类型
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="gameObject">gameObject</param>
    /// <param name="sceneName">场景名，不填为当前场景</param>
    /// <returns></returns>
    public static bool LoadComponent<T1, T2>(this GameObject gameObject, string sceneName = "")
        where T1 : ISave<T2>
        where T2 : Component
    {
        if (gameObject.CheckEmpty())
            return false;
        T2 component = gameObject.GetComponent<T2>();
        if (component.CheckEmpty())
            return false;

        SaveObject saveObject = GetSaveObjectData(gameObject.name, sceneName);
        SetValue setValue = GetComponentSetValue<T1, T2>(gameObject, saveObject);
        if (null == setValue)
            return false;
        bool result = GameSaveSystem.Load<T1, T2>( component, setValue);
        return result;
    }

    /// <summary>
    /// 读取内存中的全局数据
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。</typeparam>
    /// <typeparam name="T2">你希望保存组件对象类型
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="component">全局对象</param>
    /// <returns></returns>
    public static bool LoadGobal<T1, T2>(this T2 component)
        where T1 : ISave<T2>
    {
        if (component.CheckEmpty())
            return false;
        string componentName = typeof(T2).Name;
        GobalData gobalData = GetGobalObjectData(componentName);
        SetValue setValue = GetGobalSetValue<T1,T2>(gobalData);
        if (null == setValue)
            return false;
        bool result = GameSaveSystem.Load<T1, T2>( component, setValue);
        return result;
    }
}
