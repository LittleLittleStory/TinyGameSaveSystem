using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test
{
    [MenuItem("Tools/TestJsonSave")]
    private static void CreatFurniturePrefab()
    {
        SceneData sceneData = new SceneData("123");
        sceneData.NeedChangeObj = new Dictionary<string, ComValue>();
        ComValue comValue = new ComValue("55555");
        ComValue comValue1 = new ComValue("666");
        sceneData.NeedChangeObj.Add(comValue.Name, comValue);
        sceneData.NeedChangeObj.Add(comValue1.Name, comValue1);
        ToolUtility.SaveJson(sceneData, "test");
    }

    [MenuItem("Tools/TestJsonRead")]
    private static void CreatFurniturePrefab11()
    {
        SceneData sceneData= ToolUtility.LoadJson<SceneData>("test");
        Debug.Log(sceneData);
    }
}
