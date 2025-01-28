#if !DOTNET
using System;

namespace ET
{
    public interface IGizmosUpdate
    {
    }

    public interface IGizmosUpdateSystem: ISystemType
    {
        void Run(Entity o);
    }

    [ObjectSystem]
    public abstract class GizmosUpdateSystem<T>: IGizmosUpdateSystem where T : Entity, IGizmosUpdate
    {
        public Type Type()
        {
            return typeof (T);
        }

        public Type SystemType()
        {
            return typeof (IGizmosUpdateSystem);
        }

        public InstanceQueueIndex GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.GizmosUpdate;
        }

        public void Run(Entity o)
        {
            GizmosUpdate((T)o);
        }

        protected abstract void GizmosUpdate(T self);
    }
}
#endif