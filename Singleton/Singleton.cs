using System;

namespace ProjectBase.Base
{
    public class Singleton<T> where T : new()
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() => new T());

        public static T Instance => instance.Value;
    }

    public class GameManager : Singleton<GameManager>
    {
    }

    public class Process
    {
        public void Main()
        {
            GameManager gameManager = GameManager.Instance;
        }
    }
}