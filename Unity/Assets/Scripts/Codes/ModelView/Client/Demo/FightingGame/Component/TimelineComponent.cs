using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class TimelineComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<string, SharedVariable> paramDict = new();
    }

    //回调，调用后退出事件
    public struct CancelBehaviorCallback
    {
        public long instanceId;
    }
    
    public struct BeforeBehaviorReload
    {
        public long instanceId;
        public int behaviorOrder;
    }

    public struct AfterBehaviorReload
    {
        public long instanceId;
        public int behaviorOrder;
    }

    public struct AfterTimelineEvaluated
    {
        public long instanceId;
        public int targetFrame;
    }

    public struct OnGroundChanged
    {
        public long instanceId;
        public bool OnGround;
    }
}