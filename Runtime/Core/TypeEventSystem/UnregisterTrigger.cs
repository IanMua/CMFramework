#if UNITY
using System.Collections.Generic;
using UnityEngine;

namespace CMFramework
{
    /// <summary>
    /// 取消注册触发
    /// </summary>
    public abstract class UnregisterTrigger : MonoBehaviour
    {
        private readonly HashSet<IUnregister> _unregisters = new HashSet<IUnregister>();

        public void AddUnregister(IUnregister unregister)
        {
            _unregisters.Add(unregister);
        }

        public void RemoveUnregister(IUnregister unregister)
        {
            _unregisters.Remove(unregister);
        }

        public void Unregister()
        {
            foreach (IUnregister unregister in _unregisters)
            {
                unregister.Unregister();
            }

            _unregisters.Clear();
        }
    }

    /// <summary>
    /// 取消注册触发
    /// </summary>
    public abstract class UnregisterTrigger<T> : MonoBehaviour
    {
        private readonly HashSet<IUnregister<T>> _unregisters = new HashSet<IUnregister<T>>();

        public void AddUnregister(IUnregister<T> unregister)
        {
            _unregisters.Add(unregister);
        }

        public void RemoveUnregister(IUnregister<T> unregister)
        {
            _unregisters.Remove(unregister);
        }

        public void Unregister(T param)
        {
            foreach (IUnregister<T> unregister in _unregisters)
            {
                unregister.Unregister(param);
            }

            _unregisters.Clear();
        }
    }

    /// <summary>
    /// 取消注册触发
    /// </summary>
    public abstract class UnregisterTrigger<T1, T2> : MonoBehaviour
    {
        private readonly HashSet<IUnregister<T1, T2>> _unregisters = new HashSet<IUnregister<T1, T2>>();

        public void AddUnregister(IUnregister<T1, T2> unregister)
        {
            _unregisters.Add(unregister);
        }

        public void RemoveUnregister(IUnregister<T1, T2> unregister)
        {
            _unregisters.Remove(unregister);
        }

        public void Unregister(T1 param, T2 param2)
        {
            foreach (IUnregister<T1, T2> unregister in _unregisters)
            {
                unregister.Unregister(param, param2);
            }

            _unregisters.Clear();
        }
    }

    /// <summary>
    /// 取消注册触发
    /// </summary>
    public abstract class UnregisterTrigger<T1, T2, T3> : MonoBehaviour
    {
        private readonly HashSet<IUnregister<T1, T2, T3>> _unregisters = new HashSet<IUnregister<T1, T2, T3>>();

        public void AddUnregister(IUnregister<T1, T2, T3> unregister)
        {
            _unregisters.Add(unregister);
        }

        public void RemoveUnregister(IUnregister<T1, T2, T3> unregister)
        {
            _unregisters.Remove(unregister);
        }

        public void Unregister(T1 param, T2 param2, T3 parma3)
        {
            foreach (IUnregister<T1, T2, T3> unregister in _unregisters)
            {
                unregister.Unregister(param, param2, parma3);
            }

            _unregisters.Clear();
        }
    }
}
#endif