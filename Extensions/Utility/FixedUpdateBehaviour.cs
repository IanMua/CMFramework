using System;
using CMFramework.Core;

namespace CMFramework.Extensions.Utility
{
    /// <summary>
    /// Mono管理类
    /// 给非Mono类提供帧更新方法
    /// </summary>
    public class FixedUpdateBehaviour : MonoAutoSingleton<FixedUpdateBehaviour>
    {
        // 帧更新事件
        private event Action FixedUpdateEvent;

        private void Start()
        {
            // 不允许销毁
            DontDestroyOnLoad(FixedUpdateBehaviour.Instance.gameObject);
        }

        private void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }

        /// <summary>
        /// 添加帧更新监听
        /// </summary>
        /// <param name="action">事件方法</param>
        public IUnregister RegisterUpdate(Action action)
        {
            FixedUpdateEvent += action;

            return new FixedUpdateBehaviourUnregister()
            {
                FixedUpdateBehaviour = FixedUpdateBehaviour.Instance,
                Action = action
            };
        }

        /// <summary>
        /// 移除帧更新监听
        /// </summary>
        /// <param name="action">事件方法</param>
        public void UnregisterFixedUpdate(Action action)
        {
            FixedUpdateEvent -= action;
        }
    }

    public class FixedUpdateBehaviourUnregister : IUnregister
    {
        public FixedUpdateBehaviour FixedUpdateBehaviour;
        public Action Action;

        public void Unregister()
        {
            FixedUpdateBehaviour.UnregisterFixedUpdate(Action);

            FixedUpdateBehaviour = null;
            Action = null;
        }
    }
}