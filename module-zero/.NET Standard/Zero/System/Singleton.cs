using System;

namespace Zero.System
{
    /// <summary>
    /// Implement singleton pattern with fully lazy instantiation to prevent cross-thread issue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static readonly Lazy<T> Lazy = new Lazy<T>(() => new T());

        public static T Instance { get { return Lazy.Value; } }

        protected Singleton()
        {
        }
    }
}