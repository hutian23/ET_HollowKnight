#if !DOTNET
using System;

namespace ET
{
    public interface IFrameLateUpdate
    {
    }
    
    public interface IFrameLateUpdateSystem: ISystemType
    {
        void Run(Entity o);
    }
    
    [ObjectSystem]
    public abstract class FrameLateUpdateSystem<T>: IFrameLateUpdateSystem where T : Entity, IFrameLateUpdate
    {
        public Type Type()
        {
            return typeof (T);
        }

        public Type SystemType()
        {
            return typeof (IFrameLateUpdateSystem);
        }

        public InstanceQueueIndex GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.FrameLateUpdate;
        }

        public void Run(Entity o)
        {
            FrameLateUpdate((T)o);
        }

        protected abstract void FrameLateUpdate(T self);
    }
}
#endif