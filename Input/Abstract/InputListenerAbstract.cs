using UnityEngine.InputSystem;

namespace CMUFramework_Embark.Input.Abstract
{
    // 输入监听
    public abstract class InputListenerAbstract
    {
        // Input System Assets Map
        protected readonly InputActionMap InputActionMap;

        protected InputListenerAbstract(InputActionMap inputActionMap)
        {
            this.InputActionMap = inputActionMap;
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        public abstract void AddListenInput();

        /// <summary>
        /// 移除监听
        /// </summary>
        public abstract void RemoveListenInput();
    }
}