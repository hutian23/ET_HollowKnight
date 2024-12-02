#if !DOTNET
using System;

namespace ET
{
    public interface IFrameUpdate
    {
    }
    
    public interface IFrameUpdateSystem: ISystemType
    {
        void Run(Entity o);
    }
    
    [ObjectSystem]
    public abstract class FrameUpdateSystem<T>: IFrameUpdateSystem where T : Entity, IFrameUpdate
    {
        public Type Type()
        {
            return typeof (T);
        }

        public Type SystemType()
        {
            return typeof (IFrameUpdateSystem);
        }

        public InstanceQueueIndex GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.FrameUpdate;
        }

        public void Run(Entity o)
        {
            FrameUpdate((T)o);
        }

        protected abstract void FrameUpdate(T self);
    }
}
#endif