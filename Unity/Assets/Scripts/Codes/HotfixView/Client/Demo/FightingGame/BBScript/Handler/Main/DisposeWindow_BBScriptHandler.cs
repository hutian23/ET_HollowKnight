namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    public class DisposeWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisposeWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            
            //1. 初始化
            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.BehaviorCheckList.Clear();
            buffer.GCOptions.Clear();
            buffer.WhiffOptions.Clear();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}