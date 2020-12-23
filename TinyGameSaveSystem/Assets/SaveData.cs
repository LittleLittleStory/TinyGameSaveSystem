using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData
{
    public string SceneName { get; set; }
    public Dictionary<string, ComValue> NeedChangeObj { get; set; }
    public SceneData()
    {
        NeedChangeObj = new Dictionary<string, ComValue>();
    }

    public SceneData(string sceneName)
    {
        SceneName = sceneName;
        NeedChangeObj = new Dictionary<string, ComValue>();
    }
}

public class ComValue
{
    public string Name { get; set; }
    public Dictionary<string, Dictionary<string, SetValue>> SetValues { get; set; }
    public ComValue()
    {
        SetValues = new Dictionary<string, Dictionary<string, SetValue>>();
    }

    public ComValue(string name)
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


public interface IFunOpera
{
    void FunOpera();
}