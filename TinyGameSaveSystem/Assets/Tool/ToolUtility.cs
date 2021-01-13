using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class ToolUtility
{
    /// <summary>
    /// 序列化对象转为Json
    /// </summary>
    /// <param name="saveData"></param>
    /// <param name="path"></param>
    public static void SaveJson(object saveData, string path)
    {
        //ToJson接口将你的列表类传进去，，并自动转换为string类型
        string json = JsonMapper.ToJson(saveData);
        string filePath = string.Format("{0}/{1}.json", Application.streamingAssetsPath, path);
        //找到当前路径
        FileInfo file = new FileInfo(filePath);
        //判断有没有文件，有则打开文件，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //将转换好的字符串存进文件，
        sw.WriteLine(json);
        //注意释放资源
        sw.Close();
        sw.Dispose();
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// 反序列化Json转为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T LoadJson<T>(string path)
    {
        string textAsset;
        string filePath = string.Format("{0}/{1}.json", Application.streamingAssetsPath, path);
        if (!File.Exists(filePath))
            return default(T);
        textAsset = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(textAsset))
            return default(T);
        JsonData jsonData = JsonMapper.ToObject(textAsset);
        return JsonMapper.ToObject<T>(jsonData.ToJson());
    }

    /// <summary>
    /// 获取对应名字的子节点上的对应类型组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform">父节点 transform</param>
    /// <param name="nodeName">子节点名字</param>
    /// <returns>返回对应组件，没有找到返回null</returns>
    public static T GetComponetOnChildNode<T>(this Transform transform, string nodeName)
    {
        if (null == transform)
        {
#if UNITY_EDITOR
            string stackInfo = new System.Diagnostics.StackTrace(true).ToString();
            Debug.LogError(string.Format("{0}获取失败，路径{1}", typeof(T).ToString(), nodeName));
#endif
            return default(T);
        }

        var nodeTrans = transform.Find(nodeName);
        if (null == nodeTrans)
        {
#if UNITY_EDITOR
            string stackInfo = new System.Diagnostics.StackTrace(true).ToString();
            Debug.LogError(string.Format("{0}获取失败，路径{1}", typeof(T).ToString(), nodeName));
#endif
            return default(T);
        }

        T result = nodeTrans.GetComponent<T>();
        if (null == result)
        {
#if UNITY_EDITOR
            string stackInfo = new System.Diagnostics.StackTrace(true).ToString();
            Debug.LogError(string.Format("{0}获取失败，路径{1}", typeof(T).ToString(), nodeName));
#endif
            return default(T);
        }
        return result;
    }

    /// <summary>
    /// 检查对象是否为空
    /// </summary>
    public static bool CheckEmpty<T>(this T checkObj, int id = -1)
    {
        if (null == checkObj)
        {
#if UNITY_EDITOR
            if (id == -1)
            {
                Debug.LogError(string.Format("{0}获取失败", typeof(T).ToString()));
            }
            else
            {
                Debug.LogError(string.Format("{0}获取失败,使用ID:{1}", typeof(T).ToString(), id));
            }
#endif
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 检查数组长度
    /// </summary>
    public static bool CheckArray<T>(this T[] item, int length)
    {
        if (item.CheckEmpty())
            return false;
        if (item.Length < length)
        {
#if UNITY_EDITOR
            Debug.LogError("需求参数长度不对啊！小老弟怎么回事啊？");
#endif
            return false;
        }

        else
            return true;
    }

    /// <summary>
    /// 检查二维数组长度
    /// </summary>
    public static bool CheckArrayArray<T>(this T[][] item, int length_1, int length_2)
    {
        if (item.CheckEmpty())
            return false;
        if (item.Length < length_1)
        {
#if UNITY_EDITOR
            Debug.LogError("需求参数长度不对啊！小老弟怎么回事啊？");
#endif
            return false;
        }
        for (int i = 0; i < length_1; i++)
        {
            if (item[i].CheckEmpty())
            {
#if UNITY_EDITOR
                Debug.LogError("数组为空！小老弟怎么回事啊？");
#endif
                return false;
            }
            if (item[i].Length < length_2)
            {
#if UNITY_EDITOR
                Debug.LogError("需求参数长度不对啊！小老弟怎么回事啊？");
#endif
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检查列表长度
    /// </summary>
    public static bool CheckArray<T>(this List<T> item, int length)
    {
        if (item.CheckEmpty())
            return false;
        if (item.Count < length)
        {
#if UNITY_EDITOR
            Debug.LogError("需求参数长度不对啊！小老弟怎么回事啊？");
#endif
            return false;
        }

        else
            return true;
    }

    /// <summary>
    /// 检查二维列表长度
    /// </summary>
    public static bool CheckArrayArray<T>(this List<List<T>> item, int length_1, int length_2)
    {
        if (item.CheckEmpty())
            return false;
        if (item.Count < length_1)
        {
#if UNITY_EDITOR
            Debug.LogError("需求参数长度不对啊！小老弟怎么回事啊？");
#endif
            return false;
        }
        for (int i = 0; i < length_1; i++)
        {
            if (item[i].CheckEmpty())
            {
#if UNITY_EDITOR
                Debug.LogError("数组为空！小老弟怎么回事啊？");
#endif
                return false;
            }
            if (item[i].Count < length_2)
            {
#if UNITY_EDITOR
                Debug.LogError("需求参数长度不对啊！小老弟怎么回事啊？");
#endif
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 获得指定子物体到父物体的路径
    /// </summary>
    /// <param name="child"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static string GetNamePathToParent(Transform child, Transform parent)
    {
        if (child.CheckEmpty() || parent.CheckEmpty())
            return "";

        string path = child.name;
        Transform tempParent = child.parent;
        while (tempParent != null)
        {
            if (tempParent == parent)
                return path;
            path = string.Format("{0}/{1}", tempParent.name, path);
            tempParent = tempParent.parent;
        }
        return "";
    }

    /// <summary>
    /// 获得目标物体在场景下的完整路径
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static string GetNamePathToScene(Transform transform)
    {
        if (transform.CheckEmpty())
            return "";
        string path = transform.name;
        Transform tempParent = transform.parent;
        if (null != tempParent)
            path = transform.parent.gameObject.name + "/" + path;
        while (null != tempParent)
        {
            if (tempParent.parent != null)
            {
                tempParent = tempParent.parent;
                path = tempParent.name + "/" + path;
            }
            else
            {
                break;
            }
        }
        return path;
    } 

    /// <summary>
    /// 获得场景内所有对象包括不活跃的
    /// </summary>
    /// <returns></returns>
    public static List<GameObject> GetAllSceneObjectsWithInactive()
    {
        var allTransforms = Resources.FindObjectsOfTypeAll(typeof(Transform));
        var previousSelection = Selection.objects;
        Selection.objects = allTransforms.Cast<Transform>()
            .Where(x => x != null)
            .Select(x => x.gameObject)
            .Cast<UnityEngine.Object>().ToArray();

        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;

        return selectedTransforms.Select(tr => tr.gameObject).ToList();
    }

    /// <summary>
    /// 反射创建实例
    /// </summary>
    public static object CreateHelperInstance(string funOperaName, string[] assemblyNames)
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
