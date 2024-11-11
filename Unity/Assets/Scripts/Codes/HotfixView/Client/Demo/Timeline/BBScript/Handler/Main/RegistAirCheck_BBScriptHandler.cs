namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AirCheckTimer)]
    public class AirCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            bool ret = self.GetParent<TimelineComponent>().GetParam<bool>("OnGround");
            if (ret)
            {
                EventSystem.Instance.Invoke(new CancelBehaviorCallback(){instanceId = self.InstanceId});
            }
        }
    }

    //对于浮空的行为，需要每帧检测地面，如果落地了，取消当前行为
    public class RegistAirCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistAirCheck";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.AirCheckTimer, bbParser);
            token.Add(() => { bbTimer.Remove(ref timer); });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}