using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class GameData
{
    [SerializeField]
    public string Index { get; private set; }
    [SerializeField]
    public List<SceneData> ComValues { get; private set; }
}
public class SceneRestData
{
    public List<SceneData> ComValue;
}
[System.Serializable]
public class OtherNeedChangeObj
{
    public string sceneID;
    public List<ComValue> needChangeObjs = new List<ComValue>();
}
public class SceneJump : MonoBehaviour
{
    public string SceneJumpID;
    public string playerPostion;
    public bool stopMusic;
    public NeedSaveObj needSaveObj;
    public List<OtherNeedChangeObj> OtherSceneOBJ = new List<OtherNeedChangeObj>();
    public string sceneName;
    private SceneData comValue;

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// 保存游戏
    /// </summary>
    public void GameSave(string curDialogID = "")
    {
        /*comValue = new SceneData
        {
            SceneID = sceneName
        };
        foreach (SetComValue item in GetNeedSaveOBJ().L_SetComValue)
        {
            item.ReadComValues();
            comValue.NeedChangeObj.Add(ReadValue(item));
        }
        Save(comValue, sceneName, curDialogID);*/
    }

    public NeedSaveObj GetNeedSaveOBJ()
    {
        if (needSaveObj == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("NeedSaveOBJ");
            if (obj == null)
            {
                return null;
            }
            needSaveObj = obj.GetComponent<NeedSaveObj>();
        }
        return needSaveObj;
    }

    /// <summary>
    /// 保存JSON数据到本地的方法
    /// </summary>
    private void Save(SceneData comValue, string SceneID, string PlayerPostion, string curDialogID = "")
    {
        //ToJson接口将你的列表类传进去，，并自动转换为string类型
        string json = JsonMapper.ToJson(GetGamePlayerDatas());
        string filePath = Application.streamingAssetsPath + @"/GameSaveData.json";
        //找到当前路径
        FileInfo file = new FileInfo(filePath);
        //判断有没有文件，有则打开文件，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //将转换好的字符串存进文件，
        if (SceneJumpID != "Title")
            sw.WriteLine(json);
        //注意释放资源
        sw.Close();
        sw.Dispose();
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private ComValue ReadValue(SetComValue item)
    {
        ComValue needChangeObj = new ComValue
        {
            Name = item.gameObjectPath,
            //Values = item.L_ChangeValues,
        };
        return needChangeObj;
    }

    public static NeedSaveObj Static_GetNeedSaveOBJ()
    {
        NeedSaveObj needSaveObj;
        GameObject obj = GameObject.FindGameObjectWithTag("NeedSaveOBJ");
        if (obj == null)
        {
            return null;
        }
        needSaveObj = obj.GetComponent<NeedSaveObj>();
        return needSaveObj;
    }


    public GameObject GetTotalNodeCam()
    {
        return GameObject.FindGameObjectWithTag("TotalNodeCam");
    }

    public List<GameData> GetGamePlayerDatas()
    {
        List<GameData> gameDatas = new List<GameData>();
        foreach (GameData item in ReadComJosnValue.GetRCV().D_GameData.Values)
            gameDatas.Add(item);
        return gameDatas;
    }
}