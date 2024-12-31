using System;
using System.Collections.Generic;

namespace ET
{
    public class QueueComponent<T> : Queue<T>, IDisposable
    {
        public static QueueComponent<T> Create()
        {
            return ObjectPool.Instance.Fetch(typeof (QueueComponent<T>)) as QueueComponent<T>;
        }
        
        public void Dispose()
        {
            this.Clear();
            ObjectPool.Instance.Recycle(this);
        }
    }
}