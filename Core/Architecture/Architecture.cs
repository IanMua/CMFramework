using System;
using System.Collections.Generic;

namespace CMFramework
{
    /// <summary>
    /// 架构模块
    /// </summary>
    /// <typeparam name="T">架构类型</typeparam>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        /// <summary>
        /// 类型事件系统
        /// </summary>
        private readonly TypeEventSystem _typeEventSystem = TypeEventSystem.Instance;

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
}