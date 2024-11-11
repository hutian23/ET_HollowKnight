using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    public class MarkerEvent_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MarkerEvent";
        }

        //MarkerEvent: (Mai_GroundDash_GC);
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"MarkerEvent: \((.*?)\)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            //即用即插
            if (bbParser.GetComponent<MarkerEventParser>() == null)
            {
                bbParser.AddComponent<MarkerEventParser>();
                token.Add(bbParser.RemoveComponent<MarkerEventParser>);
            }

            //缓存动画帧事件的起始指针
            int index = parser.function_Pointers[data.functionID];
            bbParser.GetComponent<MarkerEventParser>().RegistMarker(match.Groups[1].Value, index);

            //跳过动画帧事件的代码块
            for (int i = index; i < bbParser.opDict.Count; i++)
            {
                if (bbParser.opDict[i].Equals("EndMarkerEvent:"))
                {
                    break;
                }

                bbParser.function_Pointers[data.functionID]++;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}