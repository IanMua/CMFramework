using System;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectBase.Base
{
    public class MonoAutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static Lazy<T> instance;

        public static T Instance
        {
            get
            {
                if (instance.Value == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = typeof(T).ToString();
                    instance = new Lazy<T>(gameObject.AddComponent<T>());
                }

                return instance.Value;
            }
        }
    }

    public class GameManager1 : MonoAutoSingleton<GameManager1>
    {
        public void Start()
        {
        }
    }

    public class Test
    {
        private GameManager _gameManager = GameManager.Instance;
    }
}