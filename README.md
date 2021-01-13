# TinyGameSaveSystem
基于Json序列化，为游戏提供可靠的存档系统。

只需要继承并实现指定接口，便可以存取任何你想要的组件与数值。
组件：
```csharp
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
```
保存
```csharp
transform.SaveComponent<SavePos, Transform>();
```
读取
```csharp
gameObject.LoadComponent<SavePos, Transform>();
```

全局数据
```csharp
public class GobalTest
{
    public string test;
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
```
保存
```csharp
gobalTest.SaveGobal<GobalTestDebug, GobalTest>();
```
读取
```csharp
gobalTest.LoadGobal<GobalTestDebug, GobalTest>();
```
