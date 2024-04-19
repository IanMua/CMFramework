using System;

namespace CMFramework
{
    public class TypeEventSystem : Singleton<TypeEventSystem>
    {
        private readonly EasyEvents _events = new EasyEvents();

        public void Send<T>() where T : new() => _events.GetEvent<EasyEvent<T>>()?.Trigger(new T());

        public void Send<T>(T e) => _events.GetEvent<EasyEvent<T>>()?.Trigger(e);

        public IUnregister Register<T>(Action<T> onEvent) => _events.GetOrAddEvent<EasyEvent<T>>().Register(onEvent);

        public void Unregister<T>(Action<T> onEvent)
        {
            var e = _events.GetEvent<EasyEvent<T>>();
            e?.Unregister(onEvent);
        }
    }
}