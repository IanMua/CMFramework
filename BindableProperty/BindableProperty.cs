using System;
using CMUFramework_Embark.Event;

namespace CMUFramework_Embark.BindableProperty
{
    /// <summary>
    /// 可绑定属性
    /// </summary>
    /// <remarks>Value更改后会触发类中的事件</remarks>
    /// <typeparam name="T">Type</typeparam>
    public class BindableProperty<T>
    {
        private T _value;

        private Action<T> _onValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (!value.Equals(_value))
                {
                    _value = value;

                    _onValueChanged?.Invoke(_value);
                }
            }
        }

        public IUnregister RegisterOnValueChanged(Action<T> onValueChanged)
        {
            _onValueChanged += onValueChanged;
            return new BindablePropertyUnregister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        public void UnregisterOnValueChanged(Action<T> onValueChanged)
        {
            _onValueChanged -= onValueChanged;
        }
    }

    public class BindablePropertyUnregister<T> : IUnregister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }

        public void Unregister()
        {
            BindableProperty.UnregisterOnValueChanged(OnValueChanged);

            BindableProperty = null;
            OnValueChanged = null;
        }
    }
}