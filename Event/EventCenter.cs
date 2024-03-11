using System;
using System.Collections.Generic;
using CMUFramework_Embark.Singleton;

namespace CMUFramework_Embark.Event
{
    /// <summary>
    /// Event center module
    /// </summary>
    public class EventCenter : Singleton<EventCenter>
    {
        /// <summary>
        /// 事件容器
        /// ushort 是EventEnum的常量值
        /// </summary>
        private readonly Dictionary<ushort, Action<object>> _eventContainer = new Dictionary<ushort, Action<object>>();

        /// <summary>
        /// 添加事件监听者
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">委托函数</param>
        public void AddEventListener(ushort eventType, Action<object> action)
        {
            if (_eventContainer.ContainsKey(eventType))
            {
                _eventContainer[eventType] += action;
            }
            else
            {
                _eventContainer.Add(eventType, action);
            }
        }

        /// <summary>
        /// 删除事件监听者
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">委托函数</param>
        public void RemoveEventListener(ushort eventType, Action<object> action)
        {
            if (_eventContainer.ContainsKey(eventType))
            {
                _eventContainer[eventType] -= action;
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventType">触发的事件类型</param>
        /// <param name="param">传递的参数</param>
        public void EventTrigger(ushort eventType, object param)
        {
            if (_eventContainer.ContainsKey(eventType))
            {
                _eventContainer[eventType]?.Invoke(param);
            }
        }

        /// <summary>
        /// 清除所有委托
        /// </summary>
        public void Clear()
        {
            _eventContainer.Clear();
        }
    }
}