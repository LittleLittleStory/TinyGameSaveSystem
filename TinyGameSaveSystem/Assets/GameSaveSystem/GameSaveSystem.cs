using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveSystem : Singleton<GameSaveSystem>
{
    private GameData gameData;

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

    public static void Save<T1, T2>(string name, string value, string sceneName)
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
            saveObject = GameSaveUtility.AddSaveObject(name, sceneName);


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

    public static void Save<T1, T2>(string name, string value)
        where T1 : ISave<T2>
    {
        if (string.IsNullOrEmpty(name))
            return;
        string ISaveName = typeof(T1).Name;
        string componentName = typeof(T2).Name;

        GobalData gobalData = GameSaveUtility.GetGobalSaveObjectData(name);
        if (null == gobalData)
            gobalData = GameSaveUtility.AddGobalObject(name);

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
}
