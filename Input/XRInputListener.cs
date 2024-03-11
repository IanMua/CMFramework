using CMUFramework_Embark.Event;
using CMUFramework_Embark.Extensions.Attributes;
using CMUFramework_Embark.Input.Enum;
using UnityEngine.InputSystem;

namespace CMUFramework_Embark.Input
{
    // XR输入监听模块
    public class XRInputListener : AbstractInputListener
    {
        public XRInputListener(InputActionMap inputActionMap) : base(inputActionMap)
        {
        }

        public override void ListenInput()
        {
            // 移动
            inputActionMap.FindAction(XRActionEnum.Move.GetEnumString()).performed +=
                (InputAction.CallbackContext context) =>
                {
                    EventCenter.Instance.EventTrigger(XRActionEnum.Move.GetEnumUShort(), context);
                };
            // 握把
            inputActionMap.FindAction(XRActionEnum.Grip.GetEnumString()).performed +=
                (InputAction.CallbackContext context) =>
                {
                    EventCenter.Instance.EventTrigger(XRActionEnum.Grip.GetEnumUShort(), context);
                };
            // 扳机
            inputActionMap.FindAction(XRActionEnum.Trigger.GetEnumString()).performed +=
                (InputAction.CallbackContext context) =>
                {
                    EventCenter.Instance.EventTrigger(XRActionEnum.Trigger.GetEnumUShort(), context);
                };
            // 主按键
            inputActionMap.FindAction(XRActionEnum.PrimaryButton.GetEnumString()).performed +=
                (InputAction.CallbackContext context) =>
                {
                    EventCenter.Instance.EventTrigger(XRActionEnum.PrimaryButton.GetEnumUShort(), context);
                };
            // 辅按键
            inputActionMap.FindAction(XRActionEnum.SecondaryButton.GetEnumString()).performed +=
                (InputAction.CallbackContext context) =>
                {
                    EventCenter.Instance.EventTrigger(XRActionEnum.SecondaryButton.GetEnumUShort(), context);
                };
        }
    }
}