namespace ET.Client
{
    [FriendOf(typeof(b2Unit))]
    [FriendOf(typeof(InputWait))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(parser.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
            
            Log.Warning(b2Unit.Velocity.ToString());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}