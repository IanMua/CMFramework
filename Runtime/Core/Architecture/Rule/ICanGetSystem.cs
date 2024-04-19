namespace CMFramework
{
    /// <summary>
    /// Architecture Rule 获取 System 接口
    /// </summary>
    public interface ICanGetSystem : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 获取 System 拓展方法
    /// </summary>
    public static class CanGetSystemExtension
    {
        /// <summary>
        /// 获取 System
        /// </summary>
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
}