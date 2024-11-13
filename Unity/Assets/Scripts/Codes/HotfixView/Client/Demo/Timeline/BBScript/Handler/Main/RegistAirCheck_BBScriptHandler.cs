using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AirCheckTimer)]
    public class AirCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            bool ret = self.GetParent<TimelineComponent>().GetParam<bool>("OnGround");
            if (!ret) return;
                
            int startIndex = self.GetParam<int>("AirCheck_StartIndex");
            int endIndex = self.GetParam<int>("AirCheck_EndIndex");
            long timer = self.GetParam<long>("AirCheckTimer");
            
            //销毁定时器
            BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref timer);
           
            self.RegistSubCoroutine(startIndex,endIndex,"AirCheckCallback").Coroutine();
        }
    }
    
    [FriendOf(typeof(BBParser))]
    //对于浮空的行为，需要每帧检测地面，如果落地了，取消当前行为
    public class RegistAirCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistAirCheck";
        }

        //RegistAirCheck: true
        //LogWarning: Hello World
        //WaitFrame: 30;
        //return;
        //EndAirCheck:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 
            Match match = Regex.Match(data.opLine, @"RegistAirCheck: (?<InAir>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(match.Groups["InAir"].Value);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            //2. 
            int index = bbParser.function_Pointers[data.functionID];
            int endIndex = index,startIndex = index + 1;
            while (++index < bbParser.opDict.Count)
            {
                string opLine = bbParser.opDict[index];
                if (opLine.Equals("EndAirCheck:"))
                {
                    endIndex = index;
                    break;
                }
            }
            bbParser.function_Pointers[data.functionID] = endIndex;
            
            //3. 注册变量
            bbParser.RegistParam("AirCheck_StartIndex", startIndex);
            bbParser.RegistParam("AirCheck_EndIndex", endIndex);
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.AirCheckTimer, bbParser);
            bbParser.RegistParam("AirCheckTimer", timer);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}