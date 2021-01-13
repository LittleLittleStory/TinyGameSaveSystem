using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveSystem : Singleton<GameSaveSystem>
{
    private GameData gameData;
    public static string[] assemblyNames = { "Assembly-CSharp" };

    private static GameSaveSystem gameSaveSystem
    {
        get
        {
            return SingletonManager.GetSingleton<GameSaveSystem>();
        }
    }

    public GameData GameData
    {
        get
        {
            if (null == gameData)
                gameData = ToolUtility.LoadJson<GameData>("GameData");
            if (null == gameData)
                gameData = new GameData();
            return gameData;
        }
        private set
        {
            gameData = value;
        }
    }

    public static void SaveComponent<T1, T2>(GameObject gameObject, T2 component, string sceneName)
    where T1 : ISave<T2>
    {
        if (gameObject.CheckEmpty())
            return;
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
        SaveComponent<T1, T2>(gameObject.name, value, sceneName);
    }

    public static void SaveComponent<T1, T2>(string name, string value, string sceneName)
        where T1 : ISave<T2>
    {
        if (string.IsNullOrEmpty(name))
            return;
        string ISaveName = typeof(T1).Name;
        string componentName = typeof(T2).Name;

        SceneData sceneData = GameSaveUtility.GetSceneData(sceneName);
        if (null == sceneData)
            GameSaveUtility.AddSceneData(sceneName);
        SaveObject saveObject = GameSaveUtility.GetSaveObjectData(name, sceneName);
        if (null == saveObject)
            saveObject = GameSaveUtility.AddSaveObjectData(name, sceneName);

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

        SetValue setValue = new SetValue
        {
            FunOpera = ISaveName,
            Value = value
        };
        if (setValues.ContainsKey(ISaveName))
            setValues[ISaveName] = setValue;
        else
            setValues.Add(ISaveName, setValue);
    }

    public static void SaveGobal<T1, T2>(T2 component)
        where T1 : ISave<T2>
    {
        if (component.CheckEmpty())
            return;

        ISave<T2> ISave = CreateISave<T1, T2>();
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
        string componentName = typeof(T2).Name;
        SaveGobal<T1, T2>(componentName, value);
    }

    public static void SaveGobal<T1, T2>(string componentName, string value)
        where T1 : ISave<T2>
    {
        if (string.IsNullOrEmpty(componentName))
            return;
        string ISaveName = typeof(T1).Name;

        GobalData gobalData = GameSaveUtility.GetGobalObjectData(componentName);
        if (null == gobalData)
            gobalData = GameSaveUtility.AddGobalObjectData(componentName);

        if (gobalData.SetValues.CheckEmpty())
            gobalData.SetValues = new Dictionary<string, Dictionary<string, SetValue>>();
        Dictionary<string, SetValue> setValues;
        if (gobalData.SetValues.ContainsKey(componentName))
        {
            setValues = gobalData.SetValues[componentName];
        }
        else
        {
            setValues = new Dictionary<string, SetValue>();
            gobalData.SetValues.Add(componentName, setValues);
        }

        SetValue setValue = new SetValue
        {
            FunOpera = ISaveName,
            Value = value
        };
        if (setValues.ContainsKey(ISaveName))
            setValues[ISaveName] = setValue;
        else
            setValues.Add(ISaveName, setValue);
    }

    public static bool Load<T1, T2>(T2 component, SetValue setValue)
        where T1 : ISave<T2>
    {
        if (component.CheckEmpty())
            return false;
        if (setValue.CheckEmpty())
            return false;

        ISave<T2> ISave = CreateISave<T1, T2>();
        try
        {
            ISave.Load(component, setValue.Value);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return false;
        }
        return true;
    }

    public static ISave<T2> CreateISave<T1, T2>()
       where T1 : ISave<T2>
    {
        string ISaveName = typeof(T1).Name;
        T1 ISave = (T1)ToolUtility.CreateHelperInstance(ISaveName, assemblyNames);
        return ISave;
    }
}
