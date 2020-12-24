using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Save<Transform, TestFun>(transform.position.ToString());
            GameSaveUtility.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Load<Transform, TestFun>();
        }
    }
}
public class TestFun : IFunOpera<Transform>
{
    public void FunOpera(Transform component, string value)
    {
        Debug.Log(value);
    }
}
