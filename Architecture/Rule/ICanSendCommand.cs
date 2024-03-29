using CMUFramework_Embark.Command;

namespace CMUFramework_Embark.Architecture.Rule
{
    /// <summary>
    /// Architecture Rule 发送指令接口
    /// </summary>
    public interface ICanSendCommand : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 发送指令拓展方法
    /// </summary>
    public static class CanSendCommandExtension
    {
        /// <summary>
        /// 发送指令
        /// </summary>
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand(command);
        }
    }
}