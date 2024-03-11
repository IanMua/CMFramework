using System;
using CMUFramework_Embark.Singleton;
using UnityEngine;

namespace CMUFramework_Embark.Mono
{
    /// <summary>
    /// Mono管理类
    /// 给非Mono类提供帧更新、协程等方法
    /// </summary>
    public class MonoManager : MonoAutoSingleton<MonoManager>
    {
        // 帧更新事件
        private event Action _updateEvent;

        private void Start()
        {
            // 不允许销毁
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            _updateEvent?.Invoke();
        }

        /// <summary>
        /// 添加帧更新监听
        /// </summary>
        /// <param name="action">事件方法</param>
        public void AddUpdateListener(Action action)
        {
            _updateEvent += action;
        }

        /// <summary>
        /// 移除帧更新监听
        /// </summary>
        /// <param name="action">事件方法</param>
        public void RemoveUpdateListener(Action action)
        {
            _updateEvent -= action;
        }
    }
}