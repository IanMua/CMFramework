using System.Collections.Generic;
using CMUFramework_Embark.Mono;
using CMUFramework_Embark.Singleton;

namespace CMUFramework_Embark.Input
{
    /// <summary>
    /// 输入管理模块
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        public readonly Dictionary<InputMode, InputModeState> InputModes = new Dictionary<InputMode, InputModeState>();

        // 初始化的时候就开始监听输入
        public InputManager()
        {
            // 初始化三个模式为禁用
            InputModes.Add(InputMode.KeyboardMouse, InputModeState.Disable);
            InputModes.Add(InputMode.GameController, InputModeState.Disable);
            InputModes.Add(InputMode.XRController, InputModeState.Disable);

            // 添加到帧更新
            MonoManager.Instance.AddUpdateListener(InputModeDetection);
        }

        // 输入模式检测，判断要监测哪个输入模式
        private void InputModeDetection()
        {
            if (InputModes[InputMode.KeyboardMouse] == InputModeState.Enable)
            {
                KeyboardMouse();
            }
            else if (InputModes[InputMode.GameController] == InputModeState.Enable)
            {
                GameController();
            }
            else if (InputModes[InputMode.XRController] == InputModeState.Enable)
            {
                XRController();
            }
        }

        // 键鼠控制模式
        private void KeyboardMouse()
        {
        }

        // XR控制模式
        private void XRController()
        {
        }

        // 手柄控制模式
        private void GameController()
        {
        }
    }
}