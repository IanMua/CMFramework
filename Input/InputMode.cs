using CMUFramework_Embark.Extensions.Attributes;

namespace CMUFramework_Embark.Input
{
    public enum InputMode
    {
        // 键鼠
        [EnumUShort(1)] KeyboardMouse,
        // XR
        [EnumUShort(2)] XRController,
        // 手柄
        [EnumUShort(3)] GameController,
    }
}