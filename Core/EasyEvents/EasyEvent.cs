using System;

namespace CMFramework
{
    public class EasyEvent : IEasyEvent
    {
        private Action _event;

        public IUnregister Register(Action @event)
        {
            _event += @event;
            return new CustomUnregister(() => Unregister(@event));
        }

        public void Unregister(Action @event)
        {
            _event -= @event;
        }

        public void Trigger()
        {
            _event?.Invoke();
        }
    }

    public class EasyEvent<T> : IEasyEvent
    {
        private Action<T> _event;

        public IUnregister Register(Action<T> @event)
        {
            _event += @event;
            return new CustomUnregister(() => Unregister(@event));
        }

        public void Unregister(Action<T> @event)
        {
            _event -= @event;
        }

        public void Trigger(T param)
        {
            _event?.Invoke(param);
        }

        IUnregister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _) => onEvent();
        }
    }

    public class EasyEvent<T, K> : IEasyEvent
    {
        private Action<T, K> _event;

        public IUnregister Register(Action<T, K> @event)
        {
            _event += @event;
            return new CustomUnregister(() => Unregister(@event));
        }

        public void Unregister(Action<T, K> @event)
        {
            _event -= @event;
        }

        public void Trigger(T param, K param2)
        {
            _event?.Invoke(param, param2);
        }

        IUnregister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _, K __) => onEvent();
        }
    }

    public class EasyEvent<T, K, S> : IEasyEvent
    {
        private Action<T, K, S> _event;

        public IUnregister Register(Action<T, K, S> @event)
        {
            _event += @event;
            return new CustomUnregister(() => Unregister(@event));
        }

        public void Unregister(Action<T, K, S> @event)
        {
            _event -= @event;
        }

        public void Trigger(T param, K param2, S param3)
        {
            _event?.Invoke(param, param2, param3);
        }

        IUnregister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _, K __, S ___) => onEvent();
        }
    }
}