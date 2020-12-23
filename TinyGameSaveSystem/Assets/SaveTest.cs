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
            transform.transform.position = new Vector3(1, 1, 1);
            transform.Save<Transform, TestFun>(transform.position.ToString());
        }
    }

    public class TestFun : IFunOpera
    {
        public void FunOpera()
        {
            throw new System.NotImplementedException();
        }
    }
}
