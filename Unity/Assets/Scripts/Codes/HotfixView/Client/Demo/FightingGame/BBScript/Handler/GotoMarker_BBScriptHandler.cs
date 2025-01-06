using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    public class GotoMarker_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GotoMarker";
        }

        //GotoMarker: 'Loop';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "GotoMarker: '(?<marker>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetInfoByOrder(buffer.GetCurrentOrder());

            int markerPointer = parser.GetMarkerPointer(info.behaviorName, match.Groups["marker"].Value);
            parser.Coroutine_Pointers[data.CoroutineID] = markerPointer;

            await TimerComponent.Instance.WaitFrameAsync(token);
            return token.IsCancel() ? Status.Failed : Status.Success;
        }
    }
}