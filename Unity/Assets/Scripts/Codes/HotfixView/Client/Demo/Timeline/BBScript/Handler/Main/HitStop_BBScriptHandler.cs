using System.Text.RegularExpressions;

namespace ET.Client
{
    public class HitStop_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        //算是出版的HitStop的想法
        //然后这个SingleStep的时候会bug
        //HitStop: 8;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitStop: (?<HitStop>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["HitStop"].Value, out int hitStop))
            {
                Log.Error($"cannot format HitStop to int!");
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent combatTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBTimerComponent SceneTimer = GameManager.Instance.DomainScene().GetComponent<BBTimerComponent>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}