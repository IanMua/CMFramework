using System;
using UnityEngine;

namespace CMUFramework_Embark.Singleton
{
    public class MonoSingleton<T> : MonoBehaviour
    {
        private static Lazy<T> instance;

        public static T Instance => instance.Value;

        protected virtual void Awake()
        {
            instance = new Lazy<T>(this);
        }
    }
}