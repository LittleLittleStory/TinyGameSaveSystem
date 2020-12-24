using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameData
{
    public Dictionary<string,SceneData> SceneDatas { get; set; }
    public Dictionary<string, GobalData> GobalDatas { get; set; }
    public GameData()
    {
        SceneDatas = new Dictionary<string, SceneData>();
        GobalDatas = new Dictionary<string, GobalData>();
    }
}

public class GobalData
{
    public Dictionary<string, SetValue> SetValues { get; set; }
    public GobalData()
    {
        SetValues = new Dictionary<string, SetValue>();
    }
}

public class SceneData
{
    public string SceneName { get; set; }
    public Dictionary<string, SaveObject> SaveObjects { get; set; }
    public SceneData()
    {
        SaveObjects = new Dictionary<string, SaveObject>();
    }

    public SceneData(string sceneName)
    {
        SceneName = sceneName;
        SaveObjects = new Dictionary<string, SaveObject>();
    }
}

public class SaveObject
{
    public string Name { get; set; }
    public Dictionary<string, Dictionary<string, SetValue>> SetValues { get; set; }
    public SaveObject()
    {
        SetValues = new Dictionary<string, Dictionary<string, SetValue>>();
    }

    public SaveObject(string name)
    {
        Name = name;
        SetValues = new Dictionary<string, Dictionary<string, SetValue>>();
    }
}

public class SetValue
{
    public string FunOpera { get; set; }
    public string Value { get; set; }
}


public interface IFunOpera<T>
{
    void FunOpera(T component,string value);
}