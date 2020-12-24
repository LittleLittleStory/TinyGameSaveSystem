# TinyGameSaveSystem
基于Json序列化，为游戏提供可靠的存档系统。
只需要继承并实现指定接口，便可以存取任何你想要的组件与数值
```csharp
public class DebugPos : IFunOpera<Transform>
{
    public void FunOpera(Transform component, string value)
    {
        Debug.Log(value);
    }
}
```
保存
```csharp
gameObject.Save<Transform, DebugPos>(transform.position.ToString());
```
读取，具体的读取操作将在**FunOpera()**方法内执行,具体如何执行需要各位自己编写~
```csharp
gameObject.Load<Transform, DebugPos>();
```
