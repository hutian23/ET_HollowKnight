using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(DialogueDispatcherComponent))]
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

            //2. 生成行为信息组件
            BehaviorInfo info = buffer.AddChild<BehaviorInfo>();
            
            //2-1 行为 order
            int order = buffer.Children.Count - 1;
            info.Timeline = timeline;
            info.behaviorOrder = order;
            info.behaviorName = timeline.timelineName;
            buffer.behaviorNameMap.Add(info.behaviorName,info.Id); //快速访问到组件
            buffer.behaviorOrderMap.Add(info.behaviorOrder, info.Id);
            buffer.infoIds.Add(info.Id);
            
            //2-2. 加载 trigger
            info.LoadSkillInfo(timeline);

            //2-3 调用行为初始化协程
            parser.RegistParam("InfoId", info.Id);
            int index = parser.function_Pointers[data.functionID];
  
            while (++index < parser.opDict.Count)
            {
                string opLine = parser.opDict[index];
                Match match2 = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match2.Success)
                {
                    DialogueHelper.ScripMatchError(opLine);
                    return Status.Failed;
                }
                
                string opType = match2.Value;
                //退出
                if (opType.Equals("EndMove"))
                {
                    break;
                }

                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler: {opType}");
                    return Status.Failed;
                }

                BBScriptData _data = BBScriptData.Create(opLine, data.functionID, null);
                Status ret = await handler.Handle(parser, _data, token);
                parser.function_Pointers[data.functionID] = index;
                
                if (token.IsCancel()) return Status.Failed;
                if (ret != Status.Success) return ret;
            }
            //跳转到EndMove外
            parser.function_Pointers[data.functionID] = index;
            parser.RemoveParam("InfoId");

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}