namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class VirtualCamera: Entity, IAwake, IDestroy, IGizmosUpdate
    {
        
    }

    public struct UpdateFollowOffsetCallback
    {
        public long instanceId; // bbParser.instanceId
        public int flip; // 朝向
    }
}