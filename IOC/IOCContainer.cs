using System;
using System.Collections.Generic;

namespace CMUFramework_Embark.IOC
{
    /// <summary>
    /// ICO容器
    /// </summary>
    public class IOCContainer
    {
        /// <summary>
        /// 容器字典
        /// </summary>
        private Dictionary<Type, object> _instance = new Dictionary<Type, object>();

        /// <summary>
        /// 向容器字典中注册
        /// </summary>
        /// <param name="instance">要注册的实例对象</param>
        /// <typeparam name="T">要注册的实例对象的类型</typeparam>
        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if (_instance.ContainsKey(key))
            {
                _instance[key] = instance;
            }
            else
            {
                _instance.Add(key, instance);
            }
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
}