#if !DOTNET
using System;

namespace ET
{
    public interface IPostStep
    {
        
    }

    public interface IPostStepSystem: ISystemType
    {
        void Run(Entity o);
    }

    public abstract class PostStepSystem<T>: IPostStepSystem where T : Entity, IPostStep
    {
        public Type Type()
        {
            return typeof (T);
        }

        public Type SystemType()
        {
            return typeof (IPostStepSystem);
        }

        public InstanceQueueIndex GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.PostStep;
        }

        public void Run(Entity o)
        {
            PosStepUpdate((T)o);
        }

        protected abstract void PosStepUpdate(T self);
    }
}
#endif