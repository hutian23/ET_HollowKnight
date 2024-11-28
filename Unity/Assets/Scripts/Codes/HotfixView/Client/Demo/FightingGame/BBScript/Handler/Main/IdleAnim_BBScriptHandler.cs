using System.Text.RegularExpressions;

namespace ET.Client
{
    public class IdleAnim_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "IdleAnim";
        }

        //IdleAnim: Rg_IdleAnim;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "IdleAnim: (?<Animation>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            string behaviorName = match.Groups["Animation"].Value;
            IdleAnimCor(timelineComponent, behaviorName, token).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask IdleAnimCor(TimelineComponent timelineComponent, string behaviorName, ETCancellationToken token)
        {
            await timelineComponent.GetComponent<BBTimerComponent>().WaitAsync(300, token);
            if (token.IsCancel()) return;
            timelineComponent.Reload(behaviorName);
        }
    }
}