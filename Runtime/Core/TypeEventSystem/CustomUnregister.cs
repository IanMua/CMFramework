using System;

namespace CMFramework
{
    public class CustomUnregister : IUnregister
    {
        private Action OnUnregister { get; set; }

        public CustomUnregister(Action onUnregister)
        {
            OnUnregister = onUnregister;
        }

        public void Unregister()
        {
            OnUnregister.Invoke();
            OnUnregister = null;
        }
    }
}