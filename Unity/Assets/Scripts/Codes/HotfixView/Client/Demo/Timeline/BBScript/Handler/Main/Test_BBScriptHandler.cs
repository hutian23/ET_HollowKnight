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
            b2Body b2Body = b2GameManager.Instance.GetBody(parser.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            Log.Warning(b2Body.body.LinearVelocity.ToString());
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}