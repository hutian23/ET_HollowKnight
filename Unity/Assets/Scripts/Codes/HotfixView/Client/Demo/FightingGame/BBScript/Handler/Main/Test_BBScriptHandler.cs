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
            
            Log.Warning(b2Body.GetFlip().ToString());
            
            if (inputWait.IsPressing(BBOperaType.LEFT) ||
                inputWait.IsPressing(BBOperaType.UPLEFT) ||
                inputWait.IsPressing(BBOperaType.DOWNLEFT))
            {
                Log.Warning("Pressing Left");
            }
            else if (inputWait.IsPressing(BBOperaType.RIGHT) ||
                     inputWait.IsPressing(BBOperaType.UPRIGHT) ||
                     inputWait.IsPressing(BBOperaType.DOWNRIGHT))
            {
                Log.Warning("Pressing Right");
            }
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}