using System;
using UnityEngine;

namespace CMUFramework_Embark.BindableProperty
{
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

    public class RefBindableProperty<T> where T : class
    {
        private T _value = default(T);

        public T Value
        {
            get => _value;
            set
            {
                if (!value.Equals(_value))
                {
                    _value = value;

                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public Action<T> OnValueChanged;
    }
}