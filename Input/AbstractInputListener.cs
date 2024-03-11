using UnityEngine.InputSystem;

namespace CMUFramework_Embark.Input
{
    // 输入监听
    public abstract class AbstractInputListener
    {
        protected readonly InputActionMap inputActionMap;

        protected AbstractInputListener(InputActionMap inputActionMap)
        {
            this.inputActionMap = inputActionMap;
        }

        public abstract void ListenInput();
    }
}