#if !DOTNET
using System;


namespace ET
{
    public interface IPreStep
    {
        
    }

    public interface IPreStepSystem: ISystemType
    {
        void Run(Entity o);
    }

    public abstract class PreStepSystem<T>: IPostStepSystem where T : Entity, IPreStep
    {
        public Type Type()
        {
            return typeof (T);
        }

        public Type SystemType()
        {
            return typeof (IPreStepSystem);
        }

        public InstanceQueueIndex GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.PreStep;
        }

        public void Run(Entity o)
        {
            PreStepUpdate((T)o);
        }

        protected abstract void PreStepUpdate(T self);
    }
}
#endif