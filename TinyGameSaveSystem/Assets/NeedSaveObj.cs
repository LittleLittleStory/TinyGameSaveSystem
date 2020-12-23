using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NeedSaveObj : MonoBehaviour
{
    [SerializeField]
    public List<SetComValue> L_SetComValue { get; set; }
    public SceneData comValue { get; set; }

#if UNITY_EDITOR
    public List<SetComValue> GetAllNeedSaveBOJ()
    {
        L_SetComValue.Clear();
        foreach (var item in GetAllSceneObjectsWithInactive())
        {
            if (item.GetComponent<SetComValue>() != null)
            {
                L_SetComValue.Add(item.GetComponent<SetComValue>());
            }
        }
        return L_SetComValue;
    }

    private List<GameObject> GetAllSceneObjectsWithInactive()
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
    /// 保存JSON数据到本地的方法
    /// </summary>
    private void SaveScene(SceneData comValue)
    {
        /*ReadComJosnValue.GetRCV().ReadSceneData();
        ReadComJosnValue.GetRCV().L_SComValue.Clear();
        if (ReadComJosnValue.GetRCV().D_SComValue.ContainsKey(SceneManager.GetActiveScene().name))
        {
            ReadComJosnValue.GetRCV().D_SComValue[SceneManager.GetActiveScene().name] = comValue;
        }
        else
        {
            ReadComJosnValue.GetRCV().D_SComValue.Add(SceneManager.GetActiveScene().name, comValue);
        }
        foreach (var item in ReadComJosnValue.GetRCV().D_SComValue.Values)
        {
            ReadComJosnValue.GetRCV().L_SComValue.Add(item);
        }*/
        //ToJson接口将你的列表类传进去，，并自动转换为string类型
        string json = JsonMapper.ToJson(GetGameSceneData());
        string filePath;
        filePath = Application.streamingAssetsPath + @"/SceneData.json";
        //找到当前路径
        FileInfo file = new FileInfo(filePath);
        //判断有没有文件，有则打开文件，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //将转换好的字符串存进文件，
        sw.WriteLine(json);
        //注意释放资源
        sw.Close();
        sw.Dispose();
        AssetDatabase.Refresh();
    }

    public SceneRestData GetGameSceneData()
    {
        SceneRestData sceneRestData = new SceneRestData
        {
            ComValue = new List<SceneData>()
        };
        sceneRestData.ComValue = ReadComJosnValue.GetRCV().L_SComValue;
        return sceneRestData;
    }

    private ComValue ReadValue(SetComValue item)
    {
        ComValue needChangeObj = new ComValue();
        needChangeObj.Name = item.gameObjectPath;
        return needChangeObj;
    }

    public void LoadSceneData()
    {
        ReadComJosnValue.GetRCV().ReadSceneData();
        for (int i = 0; i < ReadComJosnValue.GetRCV().L_SComValue.Count; i++)
        {
            if (ReadComJosnValue.GetRCV().L_SComValue[i].SceneName == SceneManager.GetActiveScene().name)
            {
                ReadComJosnValue.GetRCV().ReadCom(ReadComJosnValue.GetRCV().L_SComValue[i]);
                break;
            }
        }
    }
#endif
}