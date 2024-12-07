using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class Hit_GotoState_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStun";
        }

        //Hit_GotoState: 'KnockBack';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            FixtureData dataB = parser.GetParam<FixtureData>("HitData");
            
            b2Body b2Body = Root.Instance.Get(dataB.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            unit.GetComponent<TimelineComponent>().Reload("KnockBack");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}