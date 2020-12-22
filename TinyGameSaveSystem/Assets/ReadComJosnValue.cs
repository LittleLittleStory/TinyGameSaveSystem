using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;
public class ReadComJosnValue
{
    private static ReadComJosnValue _instance;

    public GameData gamedata;
    public Dictionary<string, SceneData> D_ComValue = new Dictionary<string, SceneData>();
    public List<SceneData> L_comValue = new List<SceneData>();

    public Dictionary<string, SceneData> D_SComValue = new Dictionary<string, SceneData>();
    public List<SceneData> L_SComValue = new List<SceneData>();

    public Dictionary<int, GameData> D_GameData = new Dictionary<int, GameData>();

    public static ReadComJosnValue GetRCV()
    {
        if (_instance == null)
        {
            _instance = new ReadComJosnValue();
        }
        return _instance;
    }
    public void JsonToObject()
    {
        D_GameData.Clear();
        string textAsset;
        if (!File.Exists(Application.streamingAssetsPath + @"/GameData/GameData.json"))
            return;
        textAsset = File.ReadAllText(Application.streamingAssetsPath + @"/GameData/GameData.json");
        if (textAsset == "")
            return;
        JsonData jsonData = JsonMapper.ToObject(textAsset);
        for (int i = 0; i < jsonData.Count; i++)
        {
            GameData GameData = JsonMapper.ToObject<GameData>(jsonData[i].ToJson());
            D_GameData.Add(Convert.ToInt32(GameData.Index), GameData);
        }
    }
    public void RestGameData()
    {
        /*D_ComValue.Clear();
        L_comValue.Clear();
        gamedata = new GameData();
        if (!D_GameData.ContainsKey(GameController.GetSaveDataIndex()))
            return;
        gamedata.PlayerSaveData = D_GameData[GameController.GetSaveDataIndex()].PlayerSaveData;
        for (int i = 0; i < D_GameData[GameController.GetSaveDataIndex()].ComValues.Count; i++)
        {
            ComValue CV = D_GameData[GameController.GetSaveDataIndex()].ComValues[i];
            D_ComValue.Add(CV.SceneID, CV);
            L_comValue.Add(CV);
        }*/
    }
    public void ReadSceneData()
    {
        D_SComValue.Clear();
        L_SComValue.Clear();
        string textAsset;
        textAsset = File.ReadAllText(Application.streamingAssetsPath + @"/GameSceneData/SceneData.json");
        if (textAsset == "")
        {
            return;
        }
        JsonData jsonData = JsonMapper.ToObject(textAsset);
        JsonData comValue = jsonData["ComValue"];
        if (comValue.Count == 0)
        {
            return;
        }
        for (int i = 0; i < comValue.Count; i++)
        {
            SceneData CV = JsonMapper.ToObject<SceneData>(comValue[i].ToJson());
            D_SComValue.Add(CV.SceneID, CV);
            L_SComValue.Add(CV);
        }
    }
    public void ReadCom(SceneData comValue)
    {
        foreach (ComValue item in comValue.NeedChangeObj)
        {
            GameObject obj = GameObject.Find(item.Name);
            if (obj == null)
            {
                string[] tempObjName = item.Name.Split('/');
                obj = GameObject.Find(tempObjName[0]);
                for (int i = 1; i < tempObjName.Length; i++)
                {
                    obj = GetOBJ(obj, tempObjName[i]);
                }
            }
            try
            {
                obj.GetComponent<SetComValue>().SetAllValue(item);
            }
            catch (Exception)
            {
                Debug.LogError(item.Name + "，未找到");
            }

        }
    }

    public GameObject GetOBJ(GameObject obj, string path)
    {
        return obj.transform.Find(path).gameObject;
    }
}