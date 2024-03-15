namespace CMUFramework_Embark.Observer
{
    // 对话系统被观察者
    public interface ISubject
    {
        /// <summary>
        /// 注册观察者
        /// </summary>
        /// <param name="observer">观察者</param>
        void RegisterObserver(IObserver observer);

        /// <summary>
        /// 移除观察者
        /// </summary>
        /// <param name="observer">观察者</param>
        void RemoveObserver(IObserver observer);

        /// <summary>
        /// 被观察者发起通信
        /// </summary>
        /// <param name="message">通信内容</param>
        void NotifyObservers(object message);
    }
}