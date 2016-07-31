using System;

namespace UserStorage.Service
{
    [Serializable]
    public class DataUpdatedEventArgs<T> : EventArgs
    {
        public T data;
    }
}
