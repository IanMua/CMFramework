using System;

namespace CMFramework
{
    public class BindableProperty<T> : IBindableProperty<T>
    {
        protected T _Value;

        private readonly BindablePropertyEventModelEnum _eventModel;

        private readonly EasyEvent<T> _onValueChanged = new EasyEvent<T>();

        public T Value
        {
            get => GetValue();
            set
            {
                if (value == null && _Value == null &&
                    (_eventModel == BindablePropertyEventModelEnum.ImmediatelyTrigger ||
                     _eventModel == BindablePropertyEventModelEnum.UpdateTrigger)) return;
                if (value != null && Comparer(value, _Value) &&
                    (_eventModel == BindablePropertyEventModelEnum.UpdateTrigger)) return;

                SetValue(value);
                _onValueChanged.Trigger(value);
            }
        }

        public BindableProperty(T defaultValue = default,
            BindablePropertyEventModelEnum eventModel = BindablePropertyEventModelEnum.UpdateTrigger)
        {
            _Value = defaultValue;
            _eventModel = eventModel;
        }

        /// <summary>
        /// 比较是否相同
        /// </summary>
        public static Func<T, T, bool> Comparer { get; set; } = (a, b) => a.Equals(b);

        public BindableProperty<T> WithComparer(Func<T, T, bool> comparer)
        {
            Comparer = comparer;
            return this;
        }

        protected virtual void SetValue(T newValue) => _Value = newValue;

        protected virtual T GetValue() => _Value;

        IUnregister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _) => onEvent();
        }

        /// <summary>
        /// 在不触发事件的情况下修改值
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValueWithoutEvent(T newValue)
        {
            _Value = newValue;
        }

        public IUnregister RegisterWithInitValue(Action<T> action)
        {
            action(_Value);
            return Register(action);
        }

        public void Unregister(Action<T> onValueChanged)
        {
            _onValueChanged.Unregister(onValueChanged);
        }

        public IUnregister Register(Action<T> onValueChanged)
        {
            return _onValueChanged.Register(onValueChanged);
        }

        public override string ToString()
        {
            return _Value.ToString();
        }
    }

    internal class ComparerAutoRegister
    {
#if UNITY_5_6_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoRegister()
        {
            BindableProperty<int>.Comparer = (a, b) => a == b;
            BindableProperty<float>.Comparer = (a, b) => a == b;
            BindableProperty<double>.Comparer = (a, b) => a == b;
            BindableProperty<string>.Comparer = (a, b) => a == b;
            BindableProperty<long>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector2>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector3>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector4>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Color>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Color32>.Comparer =
                (a, b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
            BindableProperty<UnityEngine.Bounds>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Rect>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Quaternion>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector2Int>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector3Int>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.BoundsInt>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.RangeInt>.Comparer = (a, b) => a.start == b.start && a.length == b.length;
            BindableProperty<UnityEngine.RectInt>.Comparer = (a, b) => a.Equals(b);
        }
#endif
    }
}