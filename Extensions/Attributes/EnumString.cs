using System;
using System.Reflection;

namespace CMUFramework_Embark.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumString : System.Attribute
    {
        public string value { get; }

        public EnumString(string value)
        {
            this.value = value;
        }
    }
    
    public static class EnumStringAttributeExtensions
    {
        // Enum拓展方法
        public static string GetEnumString(this System.Enum value)
        {
            Type type = value.GetType();
            string name = System.Enum.GetName(type, value);
            if (name == null) return null;

            FieldInfo field = type.GetField(name);
            if (field == null) return null;

            EnumString attribute = 
                (EnumString)System.Attribute.GetCustomAttribute(field, typeof(EnumString));
            return attribute?.value;
        }
    }
}