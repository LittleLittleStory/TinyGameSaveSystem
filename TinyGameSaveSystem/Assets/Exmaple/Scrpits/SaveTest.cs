using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.Save<DebugPos, Transform>();
            GameSaveUtility.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gameObject.Load<DebugPos, Transform>();
        }
    }
}
public class DebugPos : ISave<Transform>
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
