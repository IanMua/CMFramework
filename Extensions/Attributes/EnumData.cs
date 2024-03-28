using System;
using System.Collections.Generic;
using System.Reflection;

namespace CMUFramework_Embark.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumData : System.Attribute
    {
        public object Value { get; }

        public EnumData(object value)
        {
            this.Value = value;
        }
    }

    public static class EnumDataAttributeExtensions
    {
        // Enum拓展方法
        public static T GetData<T>(this Enum value) where T : class
        {
            Type type = value.GetType();
            string name = System.Enum.GetName(type, value);
            if (name == null) return null;

            FieldInfo field = type.GetField(name);
            if (field == null) return null;

            EnumData attribute =
                (EnumData)System.Attribute.GetCustomAttribute(field, typeof(EnumData));
            return (T)attribute?.Value;
        }

        /// <summary>
        /// 获取当前枚举下所有的特性值
        /// </summary>
        /// <typeparam name="T">要获取的枚举</typeparam>
        /// <typeparam name="TK">枚举值类型</typeparam>
        /// <returns></returns>
        public static List<TK> GetAllData<T, TK>() where T : Enum
        {
            Type enumType = typeof(T);
            List<TK> enumAttributes = new List<TK>();
            Array enumValues = Enum.GetValues(enumType);

            foreach (var value in enumValues)
            {
                string name = Enum.GetName(enumType, value);
                FieldInfo fieldInfo = enumType.GetField(name);
                EnumData attribute =
                    (EnumData)Attribute.GetCustomAttribute(fieldInfo, typeof(EnumData));
                enumAttributes.Add((TK)attribute?.Value);
            }

            return enumAttributes;
        }
    }
}