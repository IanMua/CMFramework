using System;
using System.Collections.Generic;
using UnityEngine;

namespace CMUFramework_Embark.Event
{
    /// <summary>
    /// 类型事件系统接口
    /// <remarks>定义了发送和注销事件</remarks>
    /// </summary>
    public interface ITypeEventSystem
    {
        /// <summary>
        /// 发送事件 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        void Send<T>() where T : new();

        /// <summary>
        /// 发送事件 
        /// </summary>
        /// <param name="e">实例对象</param>
        /// <typeparam name="T">Type</typeparam>
        void Send<T>(T e);

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="onEvent">事件</param>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        IUnregister Register<T>(Action<T> onEvent);

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="onEvent">事件</param>
        /// <typeparam name="T">Type</typeparam>
        void Unregister<T>(Action<T> onEvent);
    }

    /// <summary>
    /// 注销事件
    /// </summary>
    public interface IUnregister
    {
        void Unregister();
    }

    /// <summary>
    /// 类型事件系统注销
    /// <remarks>用来存放类型事件系统和注销的方法</remarks>
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public struct TypeEventSystemUnregister<T> : IUnregister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;

        public void Unregister()
        {
            TypeEventSystem.Unregister(OnEvent);

            TypeEventSystem = null;
            OnEvent = null;
        }
    }

    /// <summary>
    /// 在销毁时触发事件注销
    /// </summary>
    public class UnregisterOnDestroyTrigger : MonoBehaviour
    {
        private readonly HashSet<IUnregister> _unregisters = new HashSet<IUnregister>();

        /// <summary>
        /// 添加注销事件
        /// </summary>
        /// <param name="unregister">注销事件</param>
        public void AddUnregister(IUnregister unregister)
        {
            _unregisters.Add(unregister);
        }

        private void OnDestroy()
        {
            foreach (IUnregister unregister in _unregisters)
            {
                unregister.Unregister();
            }
        }
    }

    /// <summary>
    /// 事件注销的静态拓展
    /// </summary>
    public static class UnregisterExtension
    {
        /// <summary>
        /// 当 GameObject 的生命周期销毁后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        public static void UnregisterWhenGameObjectDestroyed(this IUnregister self, GameObject gameObject)
        {
            // 获取物体上的 <UnregisterOnDestroyTrigger> 组件
            UnregisterOnDestroyTrigger trigger = gameObject.GetComponent<UnregisterOnDestroyTrigger>();

            // 如果组件不存在，就在物体上添加组件
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnDestroyTrigger>();
            }

            // 向 GameObject 上添加注销事件
            trigger.AddUnregister(self);
        }
    }


    public class TypeEventSystem : ITypeEventSystem
    {
        /// <summary>
        /// 多次注册
        /// </summary>
        private interface IRegistrations
        {
        }

        /// <summary>
        /// 注册事件容器
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        private class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }

        private readonly Dictionary<Type, IRegistrations> _eventRegistration = new Dictionary<Type, IRegistrations>();

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            // ReSharper disable once InlineOutVariableDeclaration
            IRegistrations registrations;

            if (_eventRegistration.TryGetValue(type, out registrations))
            {
                ((Registrations<T>)registrations).OnEvent?.Invoke(e);
            }
        }

        public IUnregister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            // ReSharper disable once InlineOutVariableDeclaration
            IRegistrations registrations;

            // 尝试获取字典中的值，如果字典中不存在，新建一个事件容器
            if (!_eventRegistration.TryGetValue(type, out registrations))
            {
                registrations = new Registrations<T>();
                // 把事件容器放入到字典中
                _eventRegistration.Add(type, registrations);
            }

            // 把注册事件接口强转成事件容器，然后把需要订阅的事件订阅到事件容器中
            ((Registrations<T>)registrations).OnEvent += onEvent;

            // 返回类型事件系统的取消注册结构
            return new TypeEventSystemUnregister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }

        public void Unregister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            // ReSharper disable once InlineOutVariableDeclaration
            IRegistrations registrations;

            if (_eventRegistration.TryGetValue(type, out registrations))
            {
                ((Registrations<T>)registrations).OnEvent -= onEvent;
            }
        }
    }
}