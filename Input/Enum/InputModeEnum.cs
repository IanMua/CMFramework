using CMUFramework_Embark.Extensions.Attributes;

namespace CMUFramework_Embark.Input.Enum
{
    public enum InputModeEnum
    {
        // 键盘
        [EnumUShort(1)] Keyboard,

        // 鼠标
        [EnumUShort(2)] Mouse,

        // XR手柄
        [EnumUShort(3)] XRGamepad,

        // XR控制器（头部）
        [EnumUShort(4)] XRController,

        // 手柄
        [EnumUShort(5)] Gamepad,
    }
}