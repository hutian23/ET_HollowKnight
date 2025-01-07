namespace ET.Client
{
    public class BBRef_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BBRef";
        }

        //BBRef: Timeline;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            parser.UpdateParam("BBRef_Id", timelineComponent.InstanceId);
            parser.UpdateParam("BBRef_Name","Timeline");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}