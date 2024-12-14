using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class Hit_GotoState_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStun";
        }

        //代码块中可执行，需要注册Hurt_CollisionInfo变量
        //Hit_GotoState: 'KnockBack';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitStun: '(?<hitFlag>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            CollisionInfo info = parser.GetParam<CollisionInfo>("Hurt_CollisionInfo");
            
            //查询组件
            b2Body b2Body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            //查询对应的受击行为
            int order = buffer.GetHitStun(match.Groups["hitFlag"].Value);
            timelineComponent.Reload(order);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}