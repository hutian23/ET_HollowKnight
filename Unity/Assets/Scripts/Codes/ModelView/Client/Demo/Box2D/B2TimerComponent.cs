using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf]
    public class B2TimerComponent : Entity,IAwake,IDestroy
    {
        public readonly MultiMap<long, long> TimerId = new();
        public readonly Queue<long> timeOutTime = new();
        public readonly Queue<long> timeOutTimerId = new();
    }
}