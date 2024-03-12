using System.Collections.Generic;
using CMUFramework_Embark.Input.Abstract;
using CMUFramework_Embark.Input.Enum;
using CMUFramework_Embark.Input.Instance;
using CMUFramework_Embark.Singleton;
using CMUFramework_Embark.Tools;
using UnityEngine.InputSystem;

namespace CMUFramework_Embark.Input
{
    /// <summary>
    /// 输入管理模块
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        // 存储输入模式和输入监听器的映射关系
        private readonly Dictionary<InputModeEnum, InputListenerAbstract> _modeInstanceMapping =
            new Dictionary<InputModeEnum, InputListenerAbstract>();

        /// <summary>
        /// 创建输入模式
        /// 创建的输入模式将会被存入映射关系
        /// 如果需要停用或启用，建议使用EnableInputMode和DisableInputMode
        /// 以减少反复删除创建的内存开销
        /// </summary>
        /// <param name="inputMode">输入模式枚举</param>
        /// <param name="inputActionMap">输入Action的Maps</param>
        public void CreateInputMode(InputModeEnum inputMode, InputActionMap inputActionMap)
        {
            // 首先判断输入模式与实例映射是否存在，如果存在就不再继续添加
            if (_modeInstanceMapping.ContainsKey(inputMode))
            {
                return;
            }

            InputListenerAbstract inputListenerAbstract;

            // 判断输入模式和哪个输入监听器的实例符合，然后实例化
            switch (inputMode)
            {
                case InputModeEnum.XRGamepad:
                    inputListenerAbstract = new XRInputListener(inputActionMap);
                    break;
                default:
                    inputListenerAbstract = null;
                    break;
            }

            // 如果输入监听器实例不为空，就开始监听输入，添加到映射关系
            if (inputListenerAbstract == null) return;
            // 添加映射关系
            _modeInstanceMapping[inputMode] = inputListenerAbstract;
            // 监听输入
            inputListenerAbstract.AddListenInput();
        }

        /// <summary>
        /// 移除输入模式
        /// </summary>
        /// <param name="inputMode">输入模式枚举</param>
        public void RemoveInputMode(InputModeEnum inputMode)
        {
            // 先移除监听，否则会内存泄漏
            _modeInstanceMapping[inputMode].RemoveListenInput();
            // 删除该映射关系
            _modeInstanceMapping.Remove(inputMode);
        }

        /// <summary>
        /// 启用输入模式
        /// </summary>
        /// <param name="inputMode">输入模式枚举</param>
        public void EnableInputMode(InputModeEnum inputMode)
        {
            if (_modeInstanceMapping.TryGetValue(inputMode, out InputListenerAbstract inputListener))
            {
                inputListener.AddListenInput();
            }
        }

        /// <summary>
        /// 禁用输入模式
        /// </summary>
        /// <param name="inputMode">输入模式枚举</param>
        public void DisableInputMode(InputModeEnum inputMode)
        {
            if (_modeInstanceMapping.TryGetValue(inputMode, out InputListenerAbstract inputListener))
            {
                inputListener.RemoveListenInput();
            }
        }
    }
}