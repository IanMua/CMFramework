using System;

namespace CMFramework
{
    public interface IEasyEvent
    {
        IUnregister Register(Action onEvent);
    }
}