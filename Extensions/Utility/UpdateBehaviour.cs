using System;
using CMFramework.Core;
using CMFramework.Extensions.DesignPattern;

namespace CMFramework.Extensions.Utility
{
    /// <summary>
    /// Mono管理类
    /// 给非Mono类提供帧更新方法
    /// </summary>
    public class UpdateBehaviour : MonoAutoSingleton<UpdateBehaviour>
    {
        // 帧更新事件
        private event Action UpdateEvent;

        private void Start()
        {
            // 不允许销毁
            DontDestroyOnLoad(UpdateBehaviour.Instance.gameObject);
        }

        private void Update()
        {
            UpdateEvent?.Invoke();
        }

        /// <summary>
        /// 添加帧更新监听
        /// </summary>
        /// <param name="action">事件方法</param>
        public IUnregister RegisterUpdate(Action action)
        {
            UpdateEvent += action;

            return new UpdateBehaviourUnregister()
            {
                UpdateBehaviour = UpdateBehaviour.Instance,
                Action = action
            };
        }

        /// <summary>
        /// 移除帧更新监听
        /// </summary>
        /// <param name="action">事件方法</param>
        public void UnregisterUpdate(Action action)
        {
            UpdateEvent -= action;
        }
    }

    public class UpdateBehaviourUnregister : IUnregister
    {
        public UpdateBehaviour UpdateBehaviour;
        public Action Action;

        public void Unregister()
        {
            UpdateBehaviour.UnregisterUpdate(Action);

            UpdateBehaviour = null;
            Action = null;
        }
    }
}