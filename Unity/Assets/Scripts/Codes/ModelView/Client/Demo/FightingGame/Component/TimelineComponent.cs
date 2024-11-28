using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class TimelineComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<string, SharedVariable> paramDict = new();

        public Dictionary<string, long> callbackDict = new();
        public Dictionary<string, long> markerEventDict = new();
    }
    
    public struct BeforeBehaviorReload
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