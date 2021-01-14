# TinyGameSaveSystem
基于Json序列化，为游戏提供可靠的存档系统。

只需要继承并实现指定接口，便可以存取任何你想要的组件与数值。
Unity内置Mono组件或自定义Mono脚本：
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


全局数据，不继承Mono的脚本：
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
使用的Json解析库是马三大佬魔改的LitJson，链接:https://github.com/XINCGer/LitJson4Unity
