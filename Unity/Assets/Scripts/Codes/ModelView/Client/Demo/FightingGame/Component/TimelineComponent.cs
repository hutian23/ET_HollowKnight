using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class TimelineComponent: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, SharedVariable> paramDict = new();
        public Dictionary<string, long> callbackDict = new();
        public Dictionary<string, long> rootCallbackDict = new(); //在RootInit协程中注册的回调，不会随行为切换销毁
        public Dictionary<string, long> markerEventDict = new();

        public int Hertz = 60;
    }
    
    public struct BeforeBehaviorReload
    {
        public long instanceId;
        public int behaviorOrder;
    }

    public struct OnGroundChanged
    {
        public long instanceId;
        public bool OnGround;
    }
}