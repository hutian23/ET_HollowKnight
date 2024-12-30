using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(ScriptDispatcherComponent))]
    [FriendOf(typeof(BehaviorBuffer))]
    public class RootInit_RegistMove_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistMove";
        }

        //RegistMove: (Hello World)
        //    MoveType: Move;
        //    EndMove:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配BehaviorName
            Match match = Regex.Match(data.opLine, @"RegistMove: \((?<behaviorName>\w+)\)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimeline timeline = timelineComponent.GetTimelinePlayer().GetTimeline(match.Groups["behaviorName"].Value);

            //2. 注册BehaviorInfo组件到BehaviorBuffer
            BehaviorInfo info = buffer.AddChild<BehaviorInfo>();
            int order = buffer.Children.Count - 1;
            info.Timeline = timeline;
            info.behaviorOrder = order;
            info.behaviorName = match.Groups["behaviorName"].Value;
            info.LoadSkillInfo(timeline);
            buffer.behaviorNameMap.Add(info.behaviorName,info.Id); //快速访问到组件
            buffer.behaviorOrderMap.Add(info.behaviorOrder, info.Id);
            buffer.DescendInfoList.Add(info.Id);
            
            //3. 跳过Move代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndMove:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            //4. RegistMove代码块作为子协程执行
            parser.RegistParam("InfoId", info.Id);
            await parser.RegistSubCoroutine(startIndex, endIndex, token);
            if (token.IsCancel())
            {
                return Status.Failed;
            }
            parser.TryRemoveParam("InfoId");
            
            return Status.Success;
        }
    }
}