namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    [FriendOf(typeof(CameraTarget))]
    [FriendOf(typeof(VirtualCamera))]
    public class VC_Target_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Target";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            VirtualCamera vc = Root.Instance.Get(CameraManager.instance.vc_InstanceId) as VirtualCamera;

            CameraTarget target = vc.AddChild<CameraTarget>();
            target.UnitId = parser.GetParent<Unit>().InstanceId;
            target.Name = target.UnitId.ToString();
            vc.targetIds.Add(target.Name, target.Id);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}