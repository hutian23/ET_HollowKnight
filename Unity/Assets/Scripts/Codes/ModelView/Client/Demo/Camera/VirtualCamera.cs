namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class VirtualCamera: Entity, IAwake, IDestroy, IGizmosUpdate
    {
        
    }

    public struct CameraTargetInfo
    {
        public long instanceId;
    }
}