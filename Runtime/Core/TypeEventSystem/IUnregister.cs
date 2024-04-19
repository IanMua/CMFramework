namespace CMFramework
{
    /// <summary>
    /// 注销事件
    /// </summary>
    public interface IUnregister
    {
        void Unregister();
    }

    public interface IUnregister<T>
    {
        void Unregister(T param);
    }

    public interface IUnregister<T1, T2>
    {
        void Unregister(T1 param, T2 param2);
    }

    public interface IUnregister<T1, T2, T3>
    {
        void Unregister(T1 param, T2 param2, T3 param3);
    }
}