using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobalTest
{
    public string test;
}

public class SaveTest : MonoBehaviour
{
    GobalTest gobalTest = new GobalTest();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.Save<SavePos, Transform>();
            gobalTest.Save<GobalTestDebug, GobalTest>();
            GameSaveUtility.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gameObject.Load<SavePos, Transform>();
            gobalTest.Load<GobalTestDebug, GobalTest>();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            gobalTest.test = "567";
        }
    }
}
public class SavePos : ISave<Transform>
{
    public string Save(Transform component)
    {
        return JsonMapper.ToJson(component.position);
    }

    public void Load(Transform component, string value)
    {
        component.position = JsonMapper.ToObject<Vector3>(value);
    }
}

public class GobalTestDebug : ISave<GobalTest>
{
    public string Save(GobalTest component)
    {
        return component.test;
    }

    public void Load(GobalTest component, string value)
    {
        component.test = value;
        Debug.Log(component.test);
    }
}

