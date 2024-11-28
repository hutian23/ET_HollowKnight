namespace ET.Client
{
    // [Invoke(BBTimerInvokeType.CancelWindowTimer)]
    // public class CancelWindowTimer: BBTimer<BBParser>
    // {
    //     protected override void Run(BBParser self)
    //     {
    //         self.GetParent<TimelineComponent>().GetComponent<InputWait>().InputNotify();
    //     }
    // }

    [FriendOf(typeof (BehaviorInfo))]
    //一般用于普通的移动行为，比如Idle , run , Squit
    //想要实现: Idle --> run , run ---> Squit, run ---> jump, run ---> slash
    //这个是预输入窗口
    public class OpenCancelWindow_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenCancelWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.CancelWindowTimer, parser);
            bbParser.RegistParam("CancelWindowTimer", timer);
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}