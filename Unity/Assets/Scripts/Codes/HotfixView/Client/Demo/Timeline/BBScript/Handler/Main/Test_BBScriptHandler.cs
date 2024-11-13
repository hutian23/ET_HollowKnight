namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.b2Body))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            buffer.WaitHitStunNotify().Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}