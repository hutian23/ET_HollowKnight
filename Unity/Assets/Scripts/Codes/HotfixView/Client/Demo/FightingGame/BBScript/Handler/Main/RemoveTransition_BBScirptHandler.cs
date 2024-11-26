using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RemoveTransition_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveTransition";
        }

        //RemoveTransition: 'TransitionToSquit';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "RemoveTransition: '(?<transition>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            timelineComponent.GetComponent<BehaviorBuffer>().TryRemoveParam($"Transition_{match.Groups["transition"].Value}");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}