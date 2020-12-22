using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class SetComValue : MonoBehaviour
{
    public List<string> L_NeedChangeValues = new List<string>();
    public List<string> L_ChangeValues = new List<string>();
    public string gameObjectPath;
    public GameObject curParentGameobject;

    public void ReadComValues()
    {
        L_ChangeValues.Clear();
        //自定义脚本
        foreach (string item in L_NeedChangeValues)
        {
            string[] values = item.Split(',');
            string strScripteName = values[0];
            string strArriName = values[1];
            Component comp = gameObject.GetComponent(Type.GetType(strScripteName));
            if (comp != null)
            {
                System.Reflection.FieldInfo[] infp = comp.GetType().GetFields();
                for (int i = 0; i < infp.Length; i++)
                {
                    if (infp[i].Name == strArriName)
                    {
                        string value = item + "," + infp[i].GetValue(comp).ToString();
                        L_ChangeValues.Add(value);
                    }
                }
            }
        }
        SetGameObjectPath();
    }

    public void SetAllValue(ComValue needChangeObj)
    {

    }

    private void SetValue(string strScripteName, string strArriName, string strValue)
    {

    }

    public void SetGameObjectPath()
    {
        gameObjectPath = gameObject.name;
        if (gameObject.transform.parent != null)
        {
            gameObjectPath = gameObject.transform.parent.gameObject.name + "/" + gameObjectPath;
            curParentGameobject = gameObject.transform.parent.gameObject;
        }
        while (curParentGameobject != null)
        {
            if (curParentGameobject.transform.parent != null)
            {
                curParentGameobject = curParentGameobject.transform.parent.gameObject;
                gameObjectPath = curParentGameobject.name + "/" + gameObjectPath;
            }
            else
            {
                break;
            }
        }
    }
}