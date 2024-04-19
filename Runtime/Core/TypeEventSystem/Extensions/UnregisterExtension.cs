using System;
using UnityEngine;

namespace CMFramework
{
    /// <summary>
    /// 注销列表拓展
    /// </summary>
    public static class UnregisterListExtension
    {
        /// <summary>
        /// 取消所有的注册
        /// </summary>
        /// <param name="self"></param>
        public static void UnregisterAll(this IUnregisterList self)
        {
            foreach (IUnregister unregister in self.UnregisterList)
            {
                unregister.Unregister();
            }

            self.UnregisterList.Clear();
        }
    }

    /// <summary>
    /// 事件注销的静态拓展
    /// </summary>
    public static class UnregisterExtension
    {
        /// <summary>
        /// 添加注销事件到注销列表
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unregisterList"></param>
        public static void AddToUnregisterList(this IUnregister self, IUnregisterList unregisterList)
        {
            unregisterList.UnregisterList.Add(self);
        }

        /// <summary>
        /// 当 GameObject 的生命周期销毁后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        public static void UnregisterOnDestroy<T>(this IUnregister self, T component) where T : Component
        {
            self.UnregisterOnDestroy(component.gameObject);
        }

        /// <summary>
        /// 当 GameObject 被禁用后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        public static void UnregisterOnDisable<T>(this IUnregister self, T component) where T : Component
        {
            self.UnregisterOnDisable(component.gameObject);
        }

        /// <summary>
        /// 当 GameObject 的生命周期销毁后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        public static void UnregisterOnDestroy(this IUnregister self, GameObject gameObject)
        {
            // 获取物体上的 <UnregisterOnDestroy> 组件
            UnregisterOnDestroy trigger = gameObject.GetComponent<UnregisterOnDestroy>();

            // 如果组件不存在，就在物体上添加组件
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnDestroy>();
            }

            // 向 GameObject 上添加注销事件
            trigger.AddUnregister(self);
        }

        /// <summary>
        /// 当 GameObject 被禁用后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        public static void UnregisterOnDisable(this IUnregister self, GameObject gameObject)
        {
            // 获取物体上的 <UnregisterOnDestroy> 组件
            UnregisterOnDisable trigger = gameObject.GetComponent<UnregisterOnDisable>();

            // 如果组件不存在，就在物体上添加组件
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnDisable>();
            }

            // 向 GameObject 上添加注销事件
            trigger.AddUnregister(self);
        }

        /// <summary>
        /// 当 进入碰撞体 触发后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        public static void UnregisterOnCollisionEnter(this IUnregister<Collision> self, GameObject gameObject)
        {
            // 获取物体上的 <UnregisterOnDestroy> 组件
            UnregisterOnCollisionEnter trigger = gameObject.GetComponent<UnregisterOnCollisionEnter>();

            // 如果组件不存在，就在物体上添加组件
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnCollisionEnter>();
            }

            // 向 GameObject 上添加注销事件
            trigger.AddUnregister(self);
        }

        /// <summary>
        /// 当 离开碰撞体 触发后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        public static void UnregisterOnCollisionExit(this IUnregister<Collision> self, GameObject gameObject)
        {
            // 获取物体上的 <UnregisterOnDestroy> 组件
            UnregisterOnCollisionExit trigger = gameObject.GetComponent<UnregisterOnCollisionExit>();

            // 如果组件不存在，就在物体上添加组件
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnCollisionExit>();
            }

            // 向 GameObject 上添加注销事件
            trigger.AddUnregister(self);
        }

        /// <summary>
        /// 当 进入碰撞触发 触发后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        public static void UnregisterOnTriggerEnter(this IUnregister<Collider> self, GameObject gameObject)
        {
            // 获取物体上的 <UnregisterOnDestroy> 组件
            UnregisterOnTriggerEnter trigger = gameObject.GetComponent<UnregisterOnTriggerEnter>();

            // 如果组件不存在，就在物体上添加组件
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnTriggerEnter>();
            }

            // 向 GameObject 上添加注销事件
            trigger.AddUnregister(self);
        }

        /// <summary>
        /// 当 离开碰撞触发 触发后，注销相关事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameObject"></param>
        public static void UnregisterOnTriggerExit(this IUnregister<Collider> self, GameObject gameObject)
        {
            // 获取物体上的 <UnregisterOnDestroy> 组件
            UnregisterOnTriggerExit trigger = gameObject.GetComponent<UnregisterOnTriggerExit>();

            // 如果组件不存在，就在物体上添加组件
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnregisterOnTriggerExit>();
            }

            // 向 GameObject 上添加注销事件
            trigger.AddUnregister(self);
        }
    }

    /// <summary>
    /// 销毁时取消注册
    /// </summary>
    public class UnregisterOnDestroy : UnregisterTrigger
    {
        private void OnDestroy()
        {
            Unregister();
        }
    }

    /// <summary>
    /// 禁用时取消注册
    /// </summary>
    public class UnregisterOnDisable : UnregisterTrigger
    {
        private void OnDisable()
        {
            Unregister();
        }
    }

    public class UnregisterOnCollisionEnter : UnregisterTrigger<Collision>
    {
        private void OnCollisionEnter(Collision other)
        {
            Unregister(other);
        }
    }

    public class UnregisterOnCollisionExit : UnregisterTrigger<Collision>
    {
        private void OnCollisionExit(Collision other)
        {
            Unregister(other);
        }
    }

    public class UnregisterOnTriggerEnter : UnregisterTrigger<Collider>
    {
        private void OnTriggerEnter(Collider other)
        {
            Unregister(other);
        }
    }

    public class UnregisterOnTriggerExit : UnregisterTrigger<Collider>
    {
        private void OnTriggerExit(Collider other)
        {
            Unregister(other);
        }
    }
}