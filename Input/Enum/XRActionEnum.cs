using CMUFramework_Embark.Extensions.Attributes;

namespace CMUFramework_Embark.Input.Enum
{
    public enum XRActionEnum
    {
        // 移动
        [EnumString("Move"), EnumUShort(1001)] Move,

        // 握把
        [EnumString("Grip"), EnumUShort(1002)] Grip,

        // 扳机
        [EnumString("Trigger"), EnumUShort(1003)]
        Trigger,

        // 主按钮
        [EnumString("PrimaryButton"), EnumUShort(1004)]
        PrimaryButton,

        // 辅按钮
        [EnumString("SecondaryButton"), EnumUShort(1005)]
        SecondaryButton,
    }
}