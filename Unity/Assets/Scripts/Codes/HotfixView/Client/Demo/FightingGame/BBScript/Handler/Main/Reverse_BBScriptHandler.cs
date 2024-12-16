namespace ET.Client
{
    public class Reverse_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Reverse";
        }
        
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            int curFlip = body.GetFlip();
            body.SetFlip((FlipState)(-curFlip), true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}