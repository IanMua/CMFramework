using System.Collections.Generic;

namespace CMFramework
{
    /// <summary>
    /// 注销列表
    /// </summary>
    public interface IUnregisterList
    {
        /// <summary>
        /// 注销列表
        /// </summary>
        List<IUnregister> UnregisterList { get; }
    }

    /// <summary>
    /// 注销列表
    /// </summary>
    public interface IUnregisterList<T>
    {
        /// <summary>
        /// 注销列表
        /// </summary>
        List<IUnregister<T>> UnregisterList { get; }
    }
}