namespace CMUFramework_Embark.DesignPattern.Observer
{
    /// <summary>
    /// 观察者模式
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// 观察者接收到被观察者的通信后执行的操作
        /// </summary>
        /// <param name="message">通信内容</param>
        void Update(object message);
    }
}