using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.Save<Transform, TestFun>(transform.position.ToString());
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
