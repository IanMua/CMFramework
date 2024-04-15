using System;
using UnityEngine;

namespace CMFramework.Extensions.DesignPattern
{
    public class MonoAutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static Lazy<T> instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Lazy<T>(() =>
                    {
                        GameObject gameObject = new GameObject();
                        gameObject.name = typeof(T).ToString();
                        return gameObject.AddComponent<T>();
                    });
                }

                return instance.Value;
            }
        }
    }
}