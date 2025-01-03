using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class TimelineComponent: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, SharedVariable> paramDict = new();
        public Dictionary<string, long> callbackDict = new();
        public Dictionary<string, long> markerEventDict = new();
        public ETCancellationToken Token = new(); // 热重载调用
        
        public int Hertz = 60;
    }
    
    public struct BeforeBehaviorReload
    {
        public long instanceId;
        public int behaviorOrder;
    }
}