namespace ET.Client
{
    // [Invoke(BBTimerInvokeType.TransitionWindowTimer)]
    // [FriendOf(typeof(BehaviorBuffer))]
    // [FriendOf(typeof(BehaviorInfo))]
    // public class TransitionCancelWindowTimer : BBTimer<BBParser>
    // {
    //     protected override void Run(BBParser self)
    //     {
    //         //在过度动画取消窗口内，行为机逻辑上已经进入中立状态，可以切换进所有行为
    //         TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
    //         BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
    //         
    //         InputWait inputWait = timelineComponent.GetComponent<InputWait>();
    //         inputWait.InputNotify();
    //
    //         foreach (int GCOption in buffer.GCOptions)
    //         {
    //             BehaviorInfo info = buffer.GetInfoByOrder(GCOption);
    //             if (info.BehaviorCheck())
    //             {
    //                 timelineComponent.Reload(info.Timeline, info.behaviorOrder);
    //                 break;
    //             }
    //         }
    //     }
    // }

    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    public class OpenTransitionWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenTransitionWindow";
        }

        //OpenTransitionWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}