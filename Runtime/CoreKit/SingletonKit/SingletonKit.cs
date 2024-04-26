using System;
#if UNITY
using UnityEngine;
#endif

namespace CMFramework
{
    public interface ISingleton
    {
        void SingleInit();
    }

    public interface ISingletonTemplate : ISingleton
    {
        void Destroy();
    }

    /// <summary>
    /// 普通单例类
    /// </summary>
    public abstract class Singleton<T> : ISingletonTemplate where T : Singleton<T>, new()
    {
        private static Lazy<T> _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance.Value;

                _instance = new Lazy<T>(() => new T());
                return _instance.Value;
            }
        }

        public virtual void SingleInit()
        {
        }

        public virtual void Destroy()
        {
            _instance = null;
        }
    }

    /// <summary>
    /// 属性单例类
    /// </summary>
    public abstract class SingletonProperty<T> where T : class, ISingleton, new()
    {
        private static Lazy<T> _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance.Value;

                _instance = new Lazy<T>(() => new T());
                return _instance.Value;
            }
        }

        public virtual void Destroy()
        {
            _instance = null;
        }
    }

#if UNITY
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingletonTemplate where T : MonoSingleton<T>
    {
        private static Lazy<T> _instance;

        public static T Instance => _instance.Value;

        protected virtual void Awake()
        {
            _instance = new Lazy<T>(this as T);
        }

        public virtual void SingleInit()
        {
        }

        public virtual void Destroy()
        {
            _instance = null;
        }

        protected virtual void OnDestroy()
        {
            Destroy();
        }
    }

    public abstract class MonoAutoSingleton<T> : MonoBehaviour, ISingletonTemplate where T : MonoBehaviour
    {
        private static Lazy<T> _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Lazy<T>(() =>
                    {
                        GameObject gameObject = new GameObject();
                        gameObject.name = typeof(T).ToString();
                        return gameObject.AddComponent<T>();
                    });
                }

                return _instance.Value;
            }
        }

        public virtual void SingleInit()
        {
        }

        public virtual void Destroy()
        {
            _instance = null;
        }
    }
#endif
}