using System;

namespace CMFramework
{
    /// <summary>
    /// 观察者
    /// </summary>
    public interface IObserverObject
    {
        /// <summary>
        /// 观察者接收到被观察者的通信后执行的操作
        /// </summary>
        /// <param name="message">通信内容</param>
        void Update(object message);
    }

    public interface IObserverType<T>
    {
        /// <summary>
        /// 观察者接收到被观察者的通信后执行的操作
        /// </summary>
        void Update(T message);
    }

    /// <summary>
    /// 被观察者
    /// </summary>
    public interface ISubjectObject
    {
        /// <summary>
        /// 注册观察者
        /// </summary>
        void RegisterObserver(Action<object> observer);


        /// <summary>
        /// 移除观察者
        /// </summary>
        void RemoveObserver(Action<object> observer);


        /// <summary>
        /// 被观察者发起通信
        /// </summary>
        void NotifyObservers(object message);
    }

    public interface ISubjectType<T>
    {
        /// <summary>
        /// 注册观察者
        /// </summary>
        void RegisterObserver(Action<T> observer);

        /// <summary>
        /// 移除观察者
        /// </summary>
        void RemoveObserver(Action<T> observer);

        /// <summary>
        /// 被观察者发起通信
        /// </summary>
        void NotifyObservers(T message);
    }

    public abstract class SubjectObject : ISubjectObject
    {
        protected event Action<object> ObserverEvent;

        public virtual void RegisterObserver(Action<object> observer)
        {
            ObserverEvent += observer;
        }

        public virtual void RemoveObserver(Action<object> observer)
        {
            ObserverEvent -= observer;
        }

        public virtual void NotifyObservers(object message)
        {
            ObserverEvent?.Invoke(message);
        }
    }

    public abstract class SubjectType<T> : ISubjectType<T>
    {
        protected event Action<T> ObserverEvent;

        public virtual void RegisterObserver(Action<T> observer)
        {
            ObserverEvent += observer;
        }

        public virtual void RemoveObserver(Action<T> observer)
        {
            ObserverEvent -= observer;
        }

        public virtual void NotifyObservers(T message)
        {
            ObserverEvent?.Invoke(message);
        }
    }
}