namespace CMFramework
{
    /// <summary>
    /// Architecture Rule 发送 Query 接口
    /// </summary>
    public interface ICanSendQuery : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 发送 Query 拓展方法
    /// </summary>
    public static class CanSendQueryExtension
    {
        /// <summary>
        /// 发送 Query
        /// </summary>
        /// <param name="self">Self</param>
        /// <param name="query">实例对象</param>
        /// <typeparam name="T">返回的结果类型</typeparam>
        public static T SendQuery<T>(this ICanSendQuery self, IQuery<T> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }
}