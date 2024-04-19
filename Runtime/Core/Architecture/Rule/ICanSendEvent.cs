namespace CMFramework
{
    /// <summary>
    /// Architecture Rule 发送事件接口
    /// </summary>
    public interface ICanSendEvent : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 发送事件拓展方法
    /// </summary>
    public static class CanSendEventExtension
    {
        /// <summary>
        /// 发送事件
        /// </summary>
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        public static void SendEvent<T>(this ICanSendEvent self, T e) where T : new()
        {
            self.GetArchitecture().SendEvent(e);
        }
    }
}