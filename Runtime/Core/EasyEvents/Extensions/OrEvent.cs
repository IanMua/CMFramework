#if UNITY
using System;
using System.Collections.Generic;

namespace CMFramework
{
    public class OrEvent : IUnregisterList
    {
        public OrEvent Or(IEasyEvent easyEvent)
        {
            easyEvent.Register(Trigger).AddToUnregisterList(this);
            return this;
        }

        private Action _onEvent = () => { };

        public IUnregister Register(Action onEvent)
        {
            _onEvent += onEvent;
            return new CustomUnregister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action onEvent)
        {
            _onEvent -= onEvent;
            this.UnregisterAll();
        }

        private void Trigger() => _onEvent?.Invoke();

        public List<IUnregister> UnregisterList { get; } = new List<IUnregister>();
    }

    public static class OrEventExtensions
    {
        public static OrEvent Or(this IEasyEvent self, IEasyEvent e) => new OrEvent().Or(self).Or(e);
    }
}
#endif