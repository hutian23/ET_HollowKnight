namespace ET.Client
{
    [Invoke(BBTimerInvokeType.Test1)]
    [FriendOfAttribute(typeof(ET.Client.HitboxComponent))]
    public class TestTimer : BBTimer<HitboxComponent>
    {
        protected override void Run(HitboxComponent self)
        {
        }
    }

    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.Test1, hitboxComponent);
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}