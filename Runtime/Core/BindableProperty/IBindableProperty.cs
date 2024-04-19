using System;

namespace CMFramework
{
    public interface IReadonlyBindableProperty<T> : IEasyEvent
    {
        T Value { get; }

        IUnregister RegisterWithInitValue(Action<T> action);

        void Unregister(Action<T> onValueChanged);

        IUnregister Register(Action<T> onValueChanged);
    }
    
    public interface IBindableProperty<T> : IReadonlyBindableProperty<T>
    {
        new T Value { get; set; }
        void SetValueWithoutEvent(T newValue);
    }
}