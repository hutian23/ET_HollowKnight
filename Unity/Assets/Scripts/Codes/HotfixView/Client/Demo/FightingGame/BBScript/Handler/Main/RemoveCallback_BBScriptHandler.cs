using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(TimelineCallback))]
    public class RemoveCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveCallback";
        }

        //RemoveCallback: 'AirCheckCallback';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RemoveCallback: '(?<Callback>\w+)'");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            //1. Get callback
            if (!bbParser.callBackDict.TryGetValue(match.Groups["Callback"].Value, out TimelineCallback callback))
            {
                Log.Error($"cannot find bbCallback: {match.Groups["Callback"].Value}");
                return Status.Failed;
            }
            
            //2. dispose bbCallback
            long timer = callback.CheckTimer;
            bbTimer.Remove(ref timer);
            bbParser.callBackDict.Remove(match.Groups["Callback"].Value);
            bbParser.RemoveChild(callback.Id);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}