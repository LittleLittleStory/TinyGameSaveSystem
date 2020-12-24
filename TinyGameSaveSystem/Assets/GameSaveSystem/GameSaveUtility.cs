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
    public static void SaveGame()
    {
        ToolUtility.SaveJson(gameSaveSystem.GameData, "GameData");
    }

    public static SceneData GetSceneData(string sceneName)
    {
        SceneData sceneData;
        if (gameSaveSystem.GameData.SceneDatas.ContainsKey(sceneName))
        {
            sceneData = gameSaveSystem.GameData.SceneDatas[sceneName];
        }
        else
        {
            sceneData = new SceneData(sceneName);
            gameSaveSystem.GameData.SceneDatas.Add(sceneName, sceneData);
        }
        return sceneData;
    }

    public static SaveObject GetSaveObjectData(string name, string sceneName = "")
    {
        sceneName = string.IsNullOrEmpty(sceneName) == true ? SceneManager.GetActiveScene().name : sceneName;
        SceneData sceneData = GetSceneData(sceneName);
        if (sceneData.CheckEmpty())
            sceneData = new SceneData();
        SaveObject saveObject;
        if (false == sceneData.SaveObjects.ContainsKey(name))
        {
            saveObject = new SaveObject(name);
            sceneData.SaveObjects.Add(name, saveObject);
        }
        else
        {
            saveObject = sceneData.SaveObjects[name];
        }
        return saveObject;
    }

    public static void Save<T1, T2>(this T1 component, string value, string sceneName = "")
        where T1 : Component
        where T2 : IFunOpera<T1>
    {
        if (component.CheckEmpty())
            return;
        if (component.gameObject.CheckEmpty())
            return;
        Save<T1, T2>(component.gameObject.name, value, sceneName);
    }

    public static void Save<T1, T2>(this GameObject gameObject, string value, string sceneName = "")
        where T2 : IFunOpera<T1>
    {
        if (gameObject.CheckEmpty())
            return;
        Save<T1, T2>(gameObject.name, value, sceneName);
    }

    private static void Save<T1, T2>(string name, string value, string sceneName)
    where T2 : IFunOpera<T1>
    {
        if (string.IsNullOrEmpty(name))
            return;
        string componentName = typeof(T1).Name;
        string funOperaName = typeof(T2).Name;
        SaveObject saveObject = GetSaveObjectData(name, sceneName);
        SetValue setValue = new SetValue
        {
            FunOpera = funOperaName,
            Value = value
        };

        if (saveObject.SetValues.CheckEmpty())
            saveObject.SetValues = new Dictionary<string, Dictionary<string, SetValue>>();
        Dictionary<string, SetValue> setValues;
        if (saveObject.SetValues.ContainsKey(componentName))
        {
            setValues = saveObject.SetValues[componentName];
        }
        else
        {
            setValues = new Dictionary<string, SetValue>();
            saveObject.SetValues.Add(componentName, setValues);
        }

        if (setValues.ContainsKey(funOperaName))
            setValues[funOperaName] = setValue;
        else
            setValues.Add(funOperaName, setValue);
    }

    public static void Load<T1, T2>(this T1 component, string sceneName = "")
        where T1 : Component
        where T2 : IFunOpera<T1>
    {
        string componentName = typeof(T1).Name;
        string funOperaName = typeof(T2).Name;
        if (component.CheckEmpty())
            return;
        if (component.gameObject.CheckEmpty())
            return;
        SaveObject saveObject = GetSaveObjectData(component.gameObject.name, sceneName);
        if (false == saveObject.SetValues.ContainsKey(componentName))
        {
            Debug.LogError(string.Format("未找到{0}对应的组件类型", componentName));
            return;
        }
        Dictionary<string, SetValue> setValues = saveObject.SetValues[componentName];
        if (false == setValues.ContainsKey(funOperaName))
        {
            Debug.LogError(string.Format("未找到{0}对应的赋值操作类型", funOperaName));
            return;
        }
        SetValue setValue = setValues[funOperaName];
        T2 funOpera = (T2)CreateHelperInstance(funOperaName, assemblyNames);
        funOpera.FunOpera(component, setValue.Value);
    }

    /// <summary>
    /// 创建FunOpera实例
    /// </summary>
    private static object CreateHelperInstance(string funOperaName, string[] assemblyNames)
    {
        foreach (string assemblyName in assemblyNames)
        {
            Assembly assembly = Assembly.Load(assemblyName);

            object instance = assembly.CreateInstance(funOperaName);
            if (instance != null)
            {
                return instance;
            }
        }
        return null;
    }
}
