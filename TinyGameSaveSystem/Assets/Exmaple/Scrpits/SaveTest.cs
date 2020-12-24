using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.Save<Transform, DebugPos>(transform.position.ToString());
            GameSaveUtility.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gameObject.Load<Transform, DebugPos>();
        }
    }
}
public class DebugPos : IFunOpera<Transform>
{
    public void FunOpera(Transform component, string value)
    {
        Debug.Log(value);
    }
}
