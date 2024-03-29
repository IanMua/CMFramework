using System;
using CMUFramework_Embark.Event;

namespace CMUFramework_Embark.Architecture.Rule
{
    /// <summary>
    /// Architecture Rule 注册事件接口
    /// </summary>
    public interface ICanRegisterEvent : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 注册事件拓展方法
    /// </summary>
    public static class CanRegisterEventExtension
    {
        /// <summary>
        /// 注册事件
        /// </summary>
        public static IUnregister RegisterEvent<T>(this ICanSendEvent self, Action<T> onEvent) where T : new()
        {
            return self.GetArchitecture().RegisterEvent(onEvent);
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        public static void UnregisterEvent<T>(this ICanSendEvent self, Action<T> onEvent) where T : new()
        {
            self.GetArchitecture().UnRegisterEvent(onEvent);
        }
    }
}