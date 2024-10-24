namespace ET.Client
{
    public class SetVelocityY_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetVelocityY";
        }

        //SetVelocityY: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            b2Body body = b2GameManager.Instance.GetBody(parser.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            body.SetVelocityY(15f);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}