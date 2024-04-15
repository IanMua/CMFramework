using System;
using System.Collections.Generic;
using UnityEngine;

namespace CMFramework.Core
{
    #region Architecture

    public interface IArchitecture
    {
        /// <summary>
        /// 发送事件
        /// </summary>
        void SendEvent<T>() where T : new();

        /// <summary>
        /// 发送事件
        /// </summary>
        void SendEvent<T>(T e);

        /// <summary>
        /// 注册事件
        /// </summary>
        IUnregister RegisterEvent<T>(Action<T> onEvent);

        /// <summary>
        /// 注销事件
        /// </summary>
        void UnRegisterEvent<T>(Action<T> onEvent);

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        void SendCommand<T>() where T : ICommand, new();

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command">实例对象</param>
        /// <typeparam name="T">Type</typeparam>
        void SendCommand<T>(T command) where T : ICommand;

        /// <summary>
        /// 发送查询
        /// </summary>
        /// <param name="query">实例对象</param>
        /// <typeparam name="TResult">查询返回类型</typeparam>
        /// <returns></returns>
        TResult SendQuery<TResult>(IQuery<TResult> query);

        /// <summary>
        /// 注册 Utility
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterUtility<T>(T utility) where T : IUtility;

        /// <summary>
        /// 获取 Utility
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetUtility<T>() where T : class, IUtility;

        /// <summary>
        /// 注册 System
        /// </summary>
        void RegisterSystem<T>(T instance) where T : ISystem;

        /// <summary>
        /// 获取 System
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        T GetSystem<T>() where T : class, ISystem;

        /// <summary>
        /// 注册 Model
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterModel<T>(T model) where T : IModel;

        /// <summary>
        /// 获取 Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetModel<T>() where T : class, IModel;
    }

    /// <summary>
    /// 架构模块
    /// </summary>
    /// <typeparam name="T">架构类型</typeparam>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        /// <summary>
        /// 类型事件系统
        /// </summary>
        private readonly ITypeEventSystem _typeEventSystem = new TypeEventSystem();

        /// <summary>
        /// 增加注册
        /// </summary>
        private static readonly Action<T> OnRegisterPatch = _ => { };

        /// <summary>
        /// 是否初始化完成
        /// </summary>
        private bool _inited;

        /// <summary>
        /// 缓存需要初始化的 System
        /// </summary>
        private readonly List<ISystem> _systems = new List<ISystem>();

        /// <summary>
        /// 缓存需要初始化的 Model
        /// </summary>
        private readonly List<IModel> _models = new List<IModel>();

        /// <summary>
        /// 架构实例对象
        /// </summary>
        private static Lazy<T> _architecture;

        /// <summary>
        /// IController 获取架构
        /// </summary>
        /// <remarks>
        /// IController 无需使用静态类，直接用该属性可以获取架构
        /// </remarks>
        public static IArchitecture Interface
        {
            get
            {
                if (_architecture?.Value == null)
                {
                    MakeSureArchitecture();
                }

                return _architecture?.Value;
            }
        }

        /// <summary>
        /// 确保架构存在
        /// </summary>
        private static void MakeSureArchitecture()
        {
            if (_architecture == null)
            {
                // 线程安全的单例
                _architecture = new Lazy<T>(() => new T());
                _architecture.Value.Init();

                OnRegisterPatch?.Invoke(_architecture.Value);

                // 初始化完成之后，把缓存的 models 进行初始化
                foreach (IModel model in _architecture.Value._models)
                {
                    model.Init();
                }

                // 初始化完成后清空缓存
                _architecture.Value._models.Clear();

                // 初始化完成之后，把缓存的 systems 进行初始化
                foreach (ISystem system in _architecture.Value._systems)
                {
                    system.Init();
                }

                // 初始化完成后清空缓存
                _architecture.Value._systems.Clear();

                // 标志为完成初始化
                _architecture.Value._inited = true;
            }
        }

        /// <summary>
        /// 让子类去初始化进行注册
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// IOC容器
        /// </summary>
        private readonly IocContainer _iocContainer = new IocContainer();

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="TT">要获取的类型</typeparam>
        /// <returns></returns>
        public static TT Get<TT>() where TT : class
        {
            MakeSureArchitecture();
            return _architecture.Value._iocContainer.Get<TT>();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="instance">注册的类型</param>
        /// <typeparam name="TT">Type</typeparam>
        public static void Register<TT>(T instance)
        {
            MakeSureArchitecture();
            _architecture.Value._iocContainer.Register(instance);
        }

        public void SendEvent<TT>() where TT : new()
        {
            _typeEventSystem.Send<TT>();
        }

        public void SendEvent<TT>(TT e)
        {
            _typeEventSystem.Send(e);
        }

        public IUnregister RegisterEvent<TT>(Action<TT> onEvent)
        {
            return _typeEventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TT>(Action<TT> onEvent)
        {
            _typeEventSystem.Unregister(onEvent);
        }

        public void SendCommand<TT>() where TT : ICommand, new()
        {
            var command = new TT();
            command.SetArchitecture(this);
            command.Execute();
            command.SetArchitecture(null);
        }

        public void SendCommand<TT>(TT command) where TT : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        public void RegisterUtility<TT>(TT utility) where TT : IUtility
        {
            _iocContainer.Register(utility);
        }

        public TT GetUtility<TT>() where TT : class, IUtility
        {
            return _iocContainer.Get<TT>();
        }

        public void RegisterSystem<TT>(TT system) where TT : ISystem
        {
            // 需要给 System 赋值
            system.SetArchitecture(this);
            _iocContainer.Register(system);

            // 如果还没有初始化，添加到缓存，等待初始化之后进行
            if (!_inited)
            {
                _systems.Add(system);
            }
            else
            {
                system.Init();
            }
        }

        public TT GetSystem<TT>() where TT : class, ISystem
        {
            return _iocContainer.Get<TT>();
        }

        public void RegisterModel<TT>(TT model) where TT : IModel
        {
            // 需要给 Model 赋值
            model.SetArchitecture(this);
            _iocContainer.Register(model);

            // 如果还没有初始化，添加到缓存，等待初始化之后进行
            if (!_inited)
            {
                _models.Add(model);
            }
            else
            {
                model.Init();
            }
        }

        public TT GetModel<TT>() where TT : class, IModel
        {
            return _iocContainer.Get<TT>();
        }
    }

    #endregion
    
    #region Query

    public interface IQuery<TResult> : ICanSetArchitecture, ICanGetModel, ICanGetSystem,
        ICanSendQuery
    {
        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T>
    {
        private IArchitecture _architecture;

        T IQuery<T>.Do()
        {
            return OnDo();
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        protected abstract T OnDo();
    }

    #endregion
    
    #region Command

    public interface ICommand : ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanGetSystem,
        ICanSendCommand, ICanSendEvent, ICanSendQuery
    {
        void Execute();

        void Undo();
    }

    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture _architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        void ICommand.Execute()
        {
            OnExecute();
        }

        void ICommand.Undo()
        {
            OnUndo();
        }

        protected abstract void OnExecute();
        protected abstract void OnUndo();
    }

    #endregion

    #region Controller

    public interface IController : ICanSendCommand, ICanGetSystem, ICanGetModel,
        ICanRegisterEvent, ICanSendQuery
    {
    }

    #endregion

    #region Model

    public interface IModel : ICanSetArchitecture, ICanGetUtility, ICanSendEvent
    {
        void Init();
    }

    public abstract class AbstractModel : IModel
    {
        private IArchitecture _architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        void IModel.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }

    #endregion
    
    #region System

    public interface ISystem : ICanSetArchitecture, ICanGetUtility, ICanGetModel, ICanSendEvent,
        ICanRegisterEvent
    {
        void Init();
    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture _architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        void ISystem.Init()
        {
            OnInit();
        }

        /// <summary>
        /// 执行初始化
        /// </summary>
        protected abstract void OnInit();
    }

    #endregion

    #region Utility

    public interface IUtility
    {
    }

    #endregion

    #region Rule

    public interface IBelongToArchitecture
    {
        /// <summary>
        /// 获取架构
        /// </summary>
        /// <returns></returns>
        IArchitecture GetArchitecture();
    }

    public interface ICanSetArchitecture
    {
        /// <summary>
        /// 给架构赋值
        /// </summary>
        /// <param name="architecture"></param>
        void SetArchitecture(IArchitecture architecture);
    }

    /// <summary>
    /// Architecture Rule 获取 Model 接口
    /// </summary>
    public interface ICanGetModel : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 获取 Model 拓展方法
    /// </summary>
    public static class CanGetModelExtension
    {
        /// <summary>
        /// 获取 Model
        /// </summary>
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }

    /// <summary>
    /// Architecture Rule 获取 System 接口
    /// </summary>
    public interface ICanGetSystem : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 获取 System 拓展方法
    /// </summary>
    public static class CanGetSystemExtension
    {
        /// <summary>
        /// 获取 System
        /// </summary>
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }

    /// <summary>
    /// Architecture Rule 获取 Utility 接口
    /// </summary>
    public interface ICanGetUtility : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 获取 Utility 拓展方法
    /// </summary>
    public static class CanGetUtilityExtension
    {
        /// <summary>
        /// 获取 Utility
        /// </summary>
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }

    /// <summary>
    /// Architecture Rule 注册事件接口
    /// </summary>
    public interface ICanRegisterEvent : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 注册事件拓展方法
    /// </summary>
    public static class CanRegisterEventExtension
    {
        /// <summary>
        /// 注册事件
        /// </summary>
        public static IUnregister RegisterEvent<T>(this ICanSendEvent self, Action<T> onEvent) where T : new()
        {
            return self.GetArchitecture().RegisterEvent(onEvent);
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        public static void UnregisterEvent<T>(this ICanSendEvent self, Action<T> onEvent) where T : new()
        {
            self.GetArchitecture().UnRegisterEvent(onEvent);
        }
    }

    /// <summary>
    /// Architecture Rule 发送指令接口
    /// </summary>
    public interface ICanSendCommand : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 发送指令拓展方法
    /// </summary>
    public static class CanSendCommandExtension
    {
        /// <summary>
        /// 发送指令
        /// </summary>
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand(command);
        }
    }

    /// <summary>
    /// Architecture Rule 发送事件接口
    /// </summary>
    public interface ICanSendEvent : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 发送事件拓展方法
    /// </summary>
    public static class CanSendEventExtension
    {
        /// <summary>
        /// 发送事件
        /// </summary>
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        public static void SendEvent<T>(this ICanSendEvent self, T e) where T : new()
        {
            self.GetArchitecture().SendEvent(e);
        }
    }

    /// <summary>
    /// Architecture Rule 发送 Query 接口
    /// </summary>
    public interface ICanSendQuery : IBelongToArchitecture
    {
    }

    /// <summary>
    /// Architecture Rule 发送 Query 拓展方法
    /// </summary>
    public static class CanSendQueryExtension
    {
        /// <summary>
        /// 发送 Query
        /// </summary>
        /// <param name="self">Self</param>
        /// <param name="query">实例对象</param>
        /// <typeparam name="T">返回的结果类型</typeparam>
        public static T SendQuery<T>(this ICanSendQuery self, IQuery<T> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }

    #endregion

    #region TypeEventSystem

    /// <summary>
    /// 类型事件系统接口
    /// <remarks>定义了发送和注销事件</remarks>
    /// </summary>
    public interface ITypeEventSystem
    {
        /// <summary>
        /// 发送事件 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        void Send<T>() where T : new();

        /// <summary>
        /// 发送事件 
        /// </summary>
        /// <param name="e">实例对象</param>
        /// <typeparam name="T">Type</typeparam>
        void Send<T>(T e);

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="onEvent">事件</param>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        IUnregister Register<T>(Action<T> onEvent);

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="onEvent">事件</param>
        /// <typeparam name="T">Type</typeparam>
        void Unregister<T>(Action<T> onEvent);
    }

    /// <summary>
    /// 注销事件
    /// </summary>
    public interface IUnregister
    {
        void Unregister();
    }

    /// <summary>
    /// 类型事件系统注销
    /// <remarks>用来存放类型事件系统和注销的方法</remarks>
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public struct TypeEventSystemUnregister<T> : IUnregister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;

        public void Unregister()
        {
            TypeEventSystem.Unregister(OnEvent);

            TypeEventSystem = null;
            OnEvent = null;
        }
    }

    /// <summary>
    /// 在销毁时触发事件注销
    /// </summary>
    public class UnregisterOnDestroyTrigger : MonoBehaviour
    {
        private readonly HashSet<IUnregister> _unregisters = new HashSet<IUnregister>();

        /// <summary>
        /// 添加注销事件
        /// </summary>
        /// <param name="unregister">注销事件</param>
        public void AddUnregister(IUnregister unregister)
        {
            _unregisters.Add(unregister);
        }

        private void OnDestroy()
        {
            foreach (IUnregister unregister in _unregisters)
            {
                unregister.Unregister();
            }
        }
    }

    /// <summary>
    /// 事件注销的静态拓展
    /// </summary>
    public static class UnregisterExtension
    {
        /// <summary>
        /// 当 GameObject 的生命周期销毁后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        public static void UnregisterWhenGameObjectDestroyed(this IUnregister self, GameObject gameObject)
        {
            // 获取物体上的 <UnregisterOnDestroyTrigger> 组件
            UnregisterOnDestroyTrigger trigger = gameObject.GetComponent<UnregisterOnDestroyTrigger>();

            // 如果组件不存在，就在物体上添加组件
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnDestroyTrigger>();
            }

            // 向 GameObject 上添加注销事件
            trigger.AddUnregister(self);
        }
    }

    /// <summary>
    /// 类型事件系统
    /// </summary>
    public class TypeEventSystem : Singleton<TypeEventSystem>, ITypeEventSystem
    {
        /// <summary>
        /// 多次注册
        /// </summary>
        private interface IRegistrations
        {
        }

        /// <summary>
        /// 注册事件容器
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        private class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = _ => { };
        }

        private readonly Dictionary<Type, IRegistrations> _eventRegistration = new Dictionary<Type, IRegistrations>();

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            // ReSharper disable once InlineOutVariableDeclaration
            IRegistrations registrations;

            if (_eventRegistration.TryGetValue(type, out registrations))
            {
                ((Registrations<T>)registrations).OnEvent?.Invoke(e);
            }
        }

        public IUnregister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            // ReSharper disable once InlineOutVariableDeclaration
            IRegistrations registrations;

            // 尝试获取字典中的值，如果字典中不存在，新建一个事件容器
            if (!_eventRegistration.TryGetValue(type, out registrations))
            {
                registrations = new Registrations<T>();
                // 把事件容器放入到字典中
                _eventRegistration.Add(type, registrations);
            }

            // 把注册事件接口强转成事件容器，然后把需要订阅的事件订阅到事件容器中
            ((Registrations<T>)registrations).OnEvent += onEvent;

            // 返回类型事件系统的取消注册结构
            return new TypeEventSystemUnregister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }

        public void Unregister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            // ReSharper disable once InlineOutVariableDeclaration
            IRegistrations registrations;

            if (_eventRegistration.TryGetValue(type, out registrations))
            {
                ((Registrations<T>)registrations).OnEvent -= onEvent;
            }
        }
    }

    /// <summary>
    /// 事件接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOnEvent<T>
    {
        /// <summary>
        /// 被处理的事件
        /// </summary>
        /// <param name="e"></param>
        void OnEvent(T e);
    }

    /// <summary>
    /// 全局事件拓展
    /// </summary>
    public static class OnGlobalEventExtension
    {
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IUnregister RegisterEvent<T>(this IOnEvent<T> self) where T : struct
        {
            return TypeEventSystem.Instance.Register<T>(self.OnEvent);
        }

        /// <summary>
        /// 取消事件注册
        /// </summary>
        /// <param name="seft"></param>
        /// <typeparam name="T"></typeparam>
        public static void UnregisterEvent<T>(this IOnEvent<T> seft) where T : struct
        {
            TypeEventSystem.Instance.Unregister<T>(seft.OnEvent);
        }

        /// <summary>
        /// 注册事件并在GameObject销毁时取消事件注册
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IUnregister RegisterAndUnregisterWhenDestroyed<T>(this IOnEvent<T> self,
            GameObject gameObject)
            where T : struct
        {
            IUnregister unregister = TypeEventSystem.Instance.Register<T>(self.OnEvent);
            unregister.UnregisterWhenGameObjectDestroyed(gameObject);
            return unregister;
        }
    }

    #endregion

    #region `

    /// <summary>
    /// ICO容器
    /// </summary>
    public class IocContainer
    {
        /// <summary>
        /// 容器字典
        /// </summary>
        private readonly Dictionary<Type, object> _instance = new Dictionary<Type, object>();

        /// <summary>
        /// 向容器字典中注册
        /// </summary>
        /// <param name="instance">要注册的实例对象</param>
        /// <typeparam name="T">要注册的实例对象的类型</typeparam>
        public void Register<T>(T instance)
        {
            var key = typeof(T);

            _instance[key] = instance;
        }

        /// <summary>
        /// 从容器字典中获取
        /// </summary>
        /// <typeparam name="T">要获取对象的类型</typeparam>
        /// <returns>要获取对象类型的对象，当该对象不存在于字典中，会返回null</returns>
        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (_instance.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }

    #endregion

    #region BindableProperty

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
                if (value == null && _value == null) return;
                if (value != null && value.Equals(_value)) return;

                _value = value;
                _onValueChanged?.Invoke(_value);
            }
        }

        public BindableProperty(T defaultValue = default)
        {
            _value = defaultValue;
        }

        public IUnregister Register(Action<T> onValueChanged)
        {
            _onValueChanged += onValueChanged;
            return new BindablePropertyUnregister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        public IUnregister RegisterWithInit(Action<T> onValueChanged)
        {
            onValueChanged(Value);
            return Register(onValueChanged);
        }

        public void Unregister(Action<T> onValueChanged)
        {
            _onValueChanged -= onValueChanged;
        }

        public static implicit operator T(BindableProperty<T> property)
        {
            return property.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class BindablePropertyUnregister<T> : IUnregister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }

        public void Unregister()
        {
            BindableProperty.Unregister(OnValueChanged);

            BindableProperty = null;
            OnValueChanged = null;
        }
    }

    #endregion
}