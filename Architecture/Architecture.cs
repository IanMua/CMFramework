using System;
using CMUFramework_Embark.IOC;

namespace CMUFramework_Embark.Architecture
{
    /// <summary>
    /// 架构模块
    /// </summary>
    /// <typeparam name="T">架构类型</typeparam>
    public abstract class Architecture<T> where T : Architecture<T>, new()
    {
        /// <summary>
        /// 架构实例对象
        /// </summary>
        private static Lazy<T> _architecture;

        /// <summary>
        /// 确保架构存在
        /// </summary>
        private static void MakeSureArchitecture()
        {
            if (_architecture == null)
            {
                _architecture = new Lazy<T>(() => new T());
                _architecture.Value.Init();
            }
        }

        /// <summary>
        /// 让子类去初始化进行注册
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// IOC容器
        /// </summary>
        private readonly IOCContainer _iocContainer = new IOCContainer();

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">要获取什么类型</typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : class
        {
            MakeSureArchitecture();
            return _architecture.Value._iocContainer.Get<T>();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="instance">注册的类型</param>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>(T instance)
        {
            MakeSureArchitecture();
            _architecture.Value._iocContainer.Register<T>(instance);
        }
    }
}