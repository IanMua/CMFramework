using CMUFramework_Embark.Extensions.Attributes;

namespace CMUFramework_Embark.Input.Enum
{
    public enum InputModeEnum
    {
        // 键鼠
        [EnumUShort(1)] KeyboardMouse,
        // XR
        [EnumUShort(2)] XRController,
        // 手柄
        [EnumUShort(3)] GameController,
    }
}