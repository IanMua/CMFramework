using System;
using UnityEngine;

namespace CMFramework.Extensions.DesignPattern
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static Lazy<T> instance;

        public static T Instance => instance.Value;

        protected virtual void Awake()
        {
            instance = new Lazy<T>(this as T);
        }
    }
}