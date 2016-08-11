using System;

namespace UserStorage.Service
{
    /// <summary>
    /// Event that occurs when data in repository updated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class DataUpdatedEventArgs<T> : EventArgs
    {
        public T data;
    }
}
