using System.Collections.Generic;
using CMUFramework_Embark.Input.Enum;
using CMUFramework_Embark.Mono;
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
        private readonly Dictionary<InputModeEnum, InputModeStateEnum> _inputModes =
            new Dictionary<InputModeEnum, InputModeStateEnum>();

        private readonly Dictionary<string, AbstractInputListener> _inputModeListeners =
            new Dictionary<string, AbstractInputListener>();

        private AbstractInputListener _abstractInputListener;

        private XRIDefaultInputActions _xriDefaultInputActions = new XRIDefaultInputActions();

        // 初始化的时候就开始监听输入
        public InputManager()
        {
            // 初始化三个模式为禁用
            _inputModes.Add(InputModeEnum.KeyboardMouse, InputModeStateEnum.Disable);
            _inputModes.Add(InputModeEnum.GameController, InputModeStateEnum.Disable);
            _inputModes.Add(InputModeEnum.XRController, InputModeStateEnum.Disable);
        }

        /// <summary>
        /// 启用输入模式
        /// </summary>
        /// <param name="inputMode">输入模式枚举</param>
        public void EnableInputMode(InputModeEnum inputMode)
        {
            _inputModes[inputMode] = InputModeStateEnum.Enable;
        }

        /// <summary>
        /// 禁用输入模式
        /// </summary>
        /// <param name="inputMode">输入模式枚举</param>
        public void DisableInputMode(InputModeEnum inputMode)
        {
            _inputModes[inputMode] = InputModeStateEnum.Disable;
        }

        // 输入模式检测，判断要监测哪个输入模式
        private void InputModeDetection()
        {
            if (_inputModes[InputModeEnum.KeyboardMouse] == InputModeStateEnum.Enable)
            {
                if (_inputModeListeners.ContainsKey(_xriDefaultInputActions.XRILeftHandInteraction.ToString()))
                {
                    
                }
                _abstractInputListener = new KeyboardMouseInputListener(_xriDefaultInputActions.XRILeftHandInteraction);
            }
            else if (_inputModes[InputModeEnum.GameController] == InputModeStateEnum.Enable)
            {
                RealtimeDetectionGameController();
            }
            else if (_inputModes[InputModeEnum.XRController] == InputModeStateEnum.Enable)
            {
                RealtimeDetectionXRController();
            }
        }

        // 初始化键鼠控制
        private void InitKeyboardMouse()
        {
        }

        // 初始化XR控制
        private void InitXRController()
        {
            LoadAsset.LoadInputActions("");
        }

        // 初始化手柄控制
        private void InitGameController()
        {
        }

        // 实时监测键鼠控制
        private void RealtimeDetectionKeyboardMouse()
        {
        }

        // 实时监测XR控制
        private void RealtimeDetectionXRController()
        {
        }

        // 实时监测手柄控制
        private void RealtimeDetectionGameController()
        {
        }
    }
}