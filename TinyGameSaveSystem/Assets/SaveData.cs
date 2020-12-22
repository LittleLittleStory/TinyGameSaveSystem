using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData
{
    public string SceneID { get; set; }
    public List<ComValue> NeedChangeObj { get; set; }
    public SceneData()
    {
        NeedChangeObj = new List<ComValue>();
    }
}

public class ComValue
{
    public string Name { get; set; }
    public Dictionary<string, ComValueOper> Values { get; set; }

    public ComValue()
    {
        Values = new Dictionary<string, ComValueOper>();
    }
}

public class ComValueOper
{
    public string Opear { get; set; }
    public string Value { get; set; }
}

public enum Opera 
{ 
    ReadWrite,
    Fun
}

public class ComValueOper
{
    public string Opear { get; set; }
    public string Value { get; set; }
}