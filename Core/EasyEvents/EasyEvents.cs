using System;
using System.Collections.Generic;

namespace CMFramework
{
    public class EasyEvents : Singleton<EasyEvents>
    {
        /// <summary>
        /// 事件容器
        /// </summary>
        private readonly Dictionary<Type, IEasyEvent> _typeEvents = new Dictionary<Type, IEasyEvent>();

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddEvent<T>() where T : IEasyEvent, new()
        {
            _typeEvents.Add(typeof(T), new T());
        }

        /// <summary>
        /// 获取事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetEvent<T>() where T : class, IEasyEvent
        {
            return _typeEvents.TryGetValue(typeof(T), out var @event) ? @event as T : default;
        }

        /// <summary>
        /// 获取或添加事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetOrAddEvent<T>() where T : class, IEasyEvent, new()
        {
            Type type = typeof(T);
            if (_typeEvents.TryGetValue(type, out var @event))
            {
                return @event as T;
            }
            else
            {
                T tInstance = new T();
                _typeEvents.Add(type, tInstance);
                return tInstance;
            }
        }
    }
}