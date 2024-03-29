namespace CMUFramework_Embark.Architecture.Rule
{
    /// <summary>
    /// Architecture Rule 获取 Model 接口
    /// </summary>
    public interface ICanGetModel : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 获取 Model 拓展方法
    /// </summary>
    public static class CanGetModelExtension
    {
        /// <summary>
        /// 获取 Model
        /// </summary>
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
}