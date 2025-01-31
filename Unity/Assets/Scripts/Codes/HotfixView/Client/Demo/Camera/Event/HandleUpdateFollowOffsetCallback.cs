namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]
    public class HandleUpdateFollowOffsetCallback : AInvokeHandler<UpdateFollowOffsetCallback>
    {
        public override void Handle(UpdateFollowOffsetCallback args)
        {
            BBParser parser = Root.Instance.Get(args.instanceId) as BBParser;
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            
            //同向，不用调整offset
            if (parser.ContainParam("VC_Follow_OffsetFlip") && parser.GetParam<int>("VC_Follow_OffsetFlip") == args.flip)
            {
                return;
            }
            
            //1. 初始化
            parser.TryRemoveParam("VC_Follow_OffsetFlip");
            if (parser.ContainParam("VC_Follow_OffsetDampingTimer"))
            {
                long _timer = parser.GetParam<long>("VC_Follow_OffsetDampingTimer");
                lateUpdateTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("VC_Follow_OffsetDampingTimer");

            //2. 注册定时器
            long timer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraOffsetMoveTimer, parser);
            parser.RegistParam("VC_Follow_OffsetFlip", args.flip);
            parser.RegistParam("VC_Follow_OffsetDampingTimer", timer);

            parser.CancellationToken.Add(() =>
            {
                lateUpdateTimer.Remove(ref timer);
            });
        }
    }
}