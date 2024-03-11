using System;
using System.Reflection;

namespace CMUFramework_Embark.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumUShort : System.Attribute
    {
        public ushort value { get; }

        public EnumUShort(ushort value)
        {
            this.value = value;
        }
    }

    public static class EnumUShortAttributeExtensions
    {
        // Enum拓展方法
        public static ushort GetEnumUShort(this System.Enum value)
        {
            Type type = value.GetType();
            string name = System.Enum.GetName(type, value);

            FieldInfo field = type.GetField(name);

            EnumUShort attribute =
                (EnumUShort)System.Attribute.GetCustomAttribute(field, typeof(EnumUShort));
            return attribute?.value ?? 0;
        }
    }
}