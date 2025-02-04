namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class VirtualCamera: Entity, IAwake, IDestroy, IGizmosUpdate, ILoad
    {
        [StaticField]
        public static VirtualCamera Instance;
    }

    public struct UpdateFollowOffsetCallback
    {
        public long instanceId; // bbParser.instanceId
        public int flip; // 朝向
    }
}