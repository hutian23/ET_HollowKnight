namespace ET.Client.RootInit
{
    public class RootInit_HP_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HP";
        }

        //HP: 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();

            timelineComponent.RegistParam("HP", 100);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}