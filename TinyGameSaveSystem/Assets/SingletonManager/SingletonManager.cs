using UnityEngine;
using System;
using System.Collections.Generic;

public class SingletonManager : MonoBehaviour
{
    private static GameObject RootObj { get; set; }

    private static List<Action> singletonReleaseList = new List<Action>();

    public void Awake()
    {
        RootObj = gameObject;
        DontDestroyOnLoad(RootObj);
        InitSingletons();
    }

    /// <summary>
    /// 在这里进行所有单例的销毁
    /// </summary>
    public void OnApplicationQuit()
    {
        for (int i = singletonReleaseList.Count - 1; i >= 0; i--)
        {
            singletonReleaseList[i]?.Invoke();
        }
    }

    /// <summary>
    /// 在这里进行所有单例的初始化
    /// </summary>
    /// <returns></returns>
    private void InitSingletons()
    {
        AddSingleton<GameSaveSystem>();
    }

    private static T AddSingleton<T>() where T : Singleton<T>
    {
        if (RootObj.GetComponent<T>() == null)
        {
            T component = RootObj.AddComponent<T>();
            component.SetInstance(component);
            component.Init();
            singletonReleaseList.Add(delegate () { component.Release(); });
            return component;
        }
        else
        {
            return RootObj.GetComponent<T>();
        }
    }

    public static T GetSingleton<T>() where T : Singleton<T>
    {
        T component = RootObj.GetComponent<T>();
        if (component == null)
            component = AddSingleton<T>();
        return component;
    }

}
