using System;
using System.Collections.Generic;
using UnityEngine;

namespace CMFramework
{
    // #region BindableProperty

    // /// <summary>
    // /// 可绑定属性
    // /// </summary>
    // /// <remarks>Value更改后会触发类中的事件</remarks>
    // /// <typeparam name="T">Type</typeparam>
    // public class BindableProperty<T>
    // {
    //     private T _value;
    //
    //     private Action<T> _onValueChanged;
    //
    //     public T Value
    //     {
    //         get => _value;
    //         set
    //         {
    //             if (value == null && _value == null) return;
    //             if (value != null && value.Equals(_value)) return;
    //
    //             _value = value;
    //             _onValueChanged?.Invoke(_value);
    //         }
    //     }
    //
    //     public BindableProperty(T defaultValue = default)
    //     {
    //         _value = defaultValue;
    //     }
    //
    //     public IUnregister Register(Action<T> onValueChanged)
    //     {
    //         _onValueChanged += onValueChanged;
    //         return new BindablePropertyUnregister<T>()
    //         {
    //             BindableProperty = this,
    //             OnValueChanged = onValueChanged
    //         };
    //     }
    //
    //     public IUnregister RegisterWithInit(Action<T> onValueChanged)
    //     {
    //         onValueChanged(Value);
    //         return Register(onValueChanged);
    //     }
    //
    //     public void Unregister(Action<T> onValueChanged)
    //     {
    //         _onValueChanged -= onValueChanged;
    //     }
    //
    //     public static implicit operator T(BindableProperty<T> property)
    //     {
    //         return property.Value;
    //     }
    //
    //     public override string ToString()
    //     {
    //         return Value.ToString();
    //     }
    // }
    //
    // public class BindablePropertyUnregister<T> : IUnregister
    // {
    //     public BindableProperty<T> BindableProperty { get; set; }
    //
    //     public Action<T> OnValueChanged { get; set; }
    //
    //     public void Unregister()
    //     {
    //         BindableProperty.Unregister(OnValueChanged);
    //
    //         BindableProperty = null;
    //         OnValueChanged = null;
    //     }
    // }
    //
    // #endregion
}