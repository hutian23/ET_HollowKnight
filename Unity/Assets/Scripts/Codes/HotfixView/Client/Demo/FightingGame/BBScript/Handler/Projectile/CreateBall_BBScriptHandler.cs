namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class CreateBall_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateBall";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.opDict.Count)
            {
                string opLine = parser.opDict[index];
                if (opLine.Equals("EndCreateBall:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;
            
            //2. 执行代码块
            Status ret = await parser.RegistSubCoroutine(startIndex, endIndex, token);
            if (token.IsCancel() || ret != Status.Success) return Status.Failed;

            //3. 
            string ballType = parser.GetParam<string>("BallType");
            // Vector2 ballPos = parser.GetParam<Vector2>("BallPos");
            // Vector2 ballStartV = parser.GetParam<Vector2>("BallStartV");
            
            return Status.Success;
        }
    }
}