using System;

namespace CMUFramework_Embark.Singleton
{
    public class Singleton<T> where T : new()
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() => new T());

        public static T Instance => instance.Value;
    }
}