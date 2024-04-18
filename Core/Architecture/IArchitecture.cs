using System;

namespace CMFramework
{
    public interface IArchitecture
    {
        #region 注册

        /// <summary>
        /// 注册 Utility
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterUtility<T>(T utility) where T : IUtility;

        /// <summary>
        /// 注册 System
        /// </summary>
        void RegisterSystem<T>(T instance) where T : ISystem;

        /// <summary>
        /// 注册 Model
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterModel<T>(T model) where T : IModel;

        #endregion

        #region 获取

        /// <summary>
        /// 获取 Utility
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetUtility<T>() where T : class, IUtility;

        /// <summary>
        /// 获取 System
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        T GetSystem<T>() where T : class, ISystem;

        /// <summary>
        /// 获取 Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetModel<T>() where T : class, IModel;

        #endregion

        #region 通信

        /// <summary>
        /// 注册事件
        /// </summary>
        IUnregister RegisterEvent<T>(Action<T> onEvent);

        /// <summary>
        /// 发送事件
        /// </summary>
        void SendEvent<T>() where T : new();

        /// <summary>
        /// 发送事件
        /// </summary>
        void SendEvent<T>(T e);

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

        #endregion
    }
}