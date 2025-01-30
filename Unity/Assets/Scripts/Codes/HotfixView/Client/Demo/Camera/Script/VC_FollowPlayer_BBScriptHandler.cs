namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class VC_FollowPlayer_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowPlayer";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            Unit player = TODUnitHelper.GetPlayer(parser.ClientScene());

            //1. 初始化变量
            parser.TryRemoveParam("VC_Follow_Id");
            if (parser.ContainParam("VC_Follow_Timer"))
            {
                long _timer = parser.GetParam<long>("VC_Follow_Timer");
                lateUpdateTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("VC_Follow_Timer");
            
            //2. 注册变量
            long timer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraFollowTimer, parser);
            parser.RegistParam("VC_Follow_Id", player.InstanceId);
            parser.RegistParam("VC_Follow_Timer", timer);
            
            token.Add(() =>
            {
                lateUpdateTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}