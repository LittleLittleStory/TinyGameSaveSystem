using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystemUtility
{
    public static SceneData GetSceneData(string sceneName)
    {
        return ToolUtility.LoadJson<SceneData>(sceneName);
    }

    public static void Save<T1, T2>(this T1 component, string value)
        where T1 : Component
        where T2 : IFunOpera
    {
        if (component.CheckEmpty())
            return;
        if (component.gameObject.CheckEmpty())
            return;
        Save<T1, T2>(component.gameObject.name, value);
    }

    public static void Save<T1, T2>(this GameObject gameObject, string value)
        where T2 : IFunOpera
    {
        if (gameObject.CheckEmpty())
            return;
        Save<T1, T2>(gameObject.name, value);
    }

    private static void Save<T1, T2>(string name, string value)
        where T2 : IFunOpera
    {
        if (string.IsNullOrEmpty(name))
            return;
        SceneData sceneData = GetSceneData("test");
        if (sceneData.CheckEmpty())
            sceneData = new SceneData();
        ComValue comValue;
        if (false == sceneData.NeedChangeObj.ContainsKey(name))
        {
            comValue = new ComValue(name);
            sceneData.NeedChangeObj.Add(name, comValue);
        }
        else
        {
            comValue = sceneData.NeedChangeObj[name];
        }
        Dictionary<string, SetValue> setValues;
        SetValue setValue = new SetValue
        {
            FunOpera = typeof(T2).Name,
            Value = value
        };

        if (comValue.SetValues.CheckEmpty())
            comValue.SetValues = new Dictionary<string, Dictionary<string, SetValue>>();
        if (comValue.SetValues.ContainsKey(typeof(T1).Name))
        {
            setValues = comValue.SetValues[typeof(T1).Name];
        }
        else
        {
            setValues = new Dictionary<string, SetValue>();
            comValue.SetValues.Add(typeof(T1).Name, setValues);
        }

        if (setValues.ContainsKey(typeof(T2).Name))
            setValues[typeof(T2).Name] = setValue;
        else
            setValues.Add(typeof(T2).Name, setValue);
        ToolUtility.SaveJson(sceneData, "test");
    }
}
