using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class VirtualCamera: Entity, IAwake, IDestroy, IGizmosUpdate, ILoad
    {
        public Dictionary<string, long> targetIds = new();
    }

    public struct UpdateFollowOffsetCallback
    {
        public long instanceId; // bbParser.instanceId
        public int flip; // 朝向
    }
}