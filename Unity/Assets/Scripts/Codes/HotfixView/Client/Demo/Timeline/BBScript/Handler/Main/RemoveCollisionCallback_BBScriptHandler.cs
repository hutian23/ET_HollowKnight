using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    public class RemoveCollisionCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveCollisionCallback";
        }

        //RemoveCollisionCallback: 'WaitHit';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RemoveCollisionCallback: '(?<Callback>\w+)'");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            // TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            // HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            // hitboxComponent.callbackQueue.Remove(match.Groups["Callback"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}