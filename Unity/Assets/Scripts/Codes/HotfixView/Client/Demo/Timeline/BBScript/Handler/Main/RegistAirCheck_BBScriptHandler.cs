namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AirCheckTimer)]
    public class AirCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            SkillBuffer buffer = self.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            bool ret = buffer.GetParam<bool>("OnGround");
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
            BBTimerComponent bbTimer = parser.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.AirCheckTimer, parser);
            token.Add(() => { bbTimer.Remove(ref timer); });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}