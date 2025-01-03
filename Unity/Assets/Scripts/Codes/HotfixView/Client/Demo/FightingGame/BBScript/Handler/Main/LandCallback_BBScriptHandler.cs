namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class LandCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "LandCallback";
        }

        //LandCallback:
        //LogWarning: 'Hello_World';
        //EndCallback:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 跳过Move代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndCallback:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            //2. 记录指针
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            timelineComponent.TryRemoveParam("LandCallback_StartIndex");
            timelineComponent.TryRemoveParam("LandCallback_EndIndex");
            timelineComponent.RegistParam("LandCallback_StartIndex", startIndex);
            timelineComponent.RegistParam("LandCallback_EndIndex", endIndex);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}