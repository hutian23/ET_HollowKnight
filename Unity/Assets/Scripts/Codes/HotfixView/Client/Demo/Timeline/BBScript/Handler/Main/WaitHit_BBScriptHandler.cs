namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class WaitHit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitHit";
        }

        //WaitHit: 
        //  LogWarning: 'Hello_World';
        //  EndHit:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            ObjectWait objectWait = timelineComponent.GetComponent<ObjectWait>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            //1. 清除旧回调
            objectWait.Notify(new WaitHit() { Error = WaitTypeError.Destroy });

            //2. 跳过回调代码块
            int index = bbParser.function_Pointers[data.functionID];
            int endIndex = index, startIndex = index + 1;
            while (++index < bbParser.opDict.Count)
            {
                string opLine = bbParser.opDict[index];
                if (opLine.Equals("EndHit:"))
                {
                    endIndex = index;
                    break;
                }
            }
            bbParser.function_Pointers[data.functionID] = endIndex;
            
            //3. 注册变量
            bbParser.RegistParam("WaitHit_StartIndex", startIndex);
            bbParser.RegistParam("WaitHit_EndIndex", endIndex);
            
            //2. 等待执行回调
            WaitHitCoroutine(timelineComponent).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask<Status> WaitHitCoroutine(TimelineComponent timelineComponent)
        {
            ObjectWait objectWait = timelineComponent.GetComponent<ObjectWait>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            WaitHit wait = await objectWait.Wait<WaitHit>(bbParser.cancellationToken);
            if (wait.Error != WaitTypeError.Success)
            {
                return Status.Failed;
            }
            
            int startIndex = bbParser.GetParam<int>("WaitHit_StartIndex");
            int endIndex=  bbParser.GetParam<int>("WaitHit_EndIndex");
            bbParser.RegistSubCoroutine(startIndex, endIndex, "WaitHitCallback").Coroutine();
            bbParser.RemoveParam("WaitHit_StartIndex");
            bbParser.RemoveParam("WaitHit_EndIndex");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}