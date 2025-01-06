using System.Text.RegularExpressions;

namespace ET.Client
{
    public class SetTransition_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetTransition";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SetTransition: '(?<transition>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            timelineComponent.GetComponent<BehaviorBuffer>().RegistParam($"Transition_{match.Groups["transition"].Value}", true);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}