using CMUFramework_Embark.Mono;

namespace CMUFramework_Embark.Architecture.Rule
{
    /// <summary>
    /// Architecture Rule 获取 Utility 接口
    /// </summary>
    public interface ICanGetUtility : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 获取 Utility 拓展方法
    /// </summary>
    public static class CanGetUtilityExtension
    {
        /// <summary>
        /// 获取 Utility
        /// </summary>
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }

        /// <summary>
        /// 获取 MonoManager
        /// </summary>
        /// <returns><see cref="MonoManager"/></returns>
        public static MonoManager GetMonoManager(this ICanGetUtility self)
        {
            return MonoManager.Instance;
        }
    }
}