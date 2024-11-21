using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    public class CollisionCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CollisionCallback";
        }

        //CollisionCallback: 'HitTest';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CollisionCallback: '(?<callBack>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            hitboxComponent.callbackSet.Add(match.Groups["callBack"].Value);
            //调用注册事件
            string callBackName = match.Groups["callBack"].Value;
            CollisionCallback callback = DialogueDispatcherComponent.Instance.GetCollisionCallback(callBackName);
            callback.Regist(bbParser);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}