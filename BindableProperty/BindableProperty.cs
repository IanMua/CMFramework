using System;

namespace CMUFramework_Embark.BindableProperty
{
    /// <summary>
    /// 可绑定属性
    /// </summary>
    /// <remarks>继承了 IEquatable 接口数据类型和类</remarks>
    /// <typeparam name="T">Type</typeparam>
    public class BindableProperty<T> where T : IEquatable<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                // Debug.Log(typeof(T) + "的值：" + _value + " 改变了");
                if (!value.Equals(_value))
                {
                    // Debug.Log(typeof(T) + "的值：" + _value + " 委托");
                    _value = value;

                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public Action<T> OnValueChanged;
    }

    /// <summary>
    /// 引用类型的可绑定属性
    /// </summary>
    /// <remarks>没有继承 IEquatable 接口的引用类型的可绑定属性。当值改变时，判断堆地址是否相同</remarks>
    /// <typeparam name="T">Type</typeparam>
    public class RefBindableProperty<T> where T : class
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (!System.Object.ReferenceEquals(value, _value))
                {
                    _value = value;

                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public Action<T> OnValueChanged;
    }
}