using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class Hit_GotoState_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Hit_GotoState";
        }

        //Hit_GotoState: 'KnockBack';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            switch (parser.GetParam<TriggerType>("TriggerType"))
            {
                case TriggerType.TriggerEnter:
                    {
                        TriggerEnterCallback callback = parser.GetParam<TriggerEnterCallback>("TriggerCallback");
                        BoxInfo boxInfo = callback.dataB.UserData as BoxInfo;

                        if (boxInfo.hitboxType is not HitboxType.Hurt) return Status.Success;

                        b2Body B2Body = Root.Instance.Get(callback.dataB.InstanceId) as b2Body;
                        Unit unit = Root.Instance.Get(B2Body.unitId) as Unit;

                        //通知unit切换行为
                        TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
                        timelineComponent.GetComponent<ObjectWait>().Notify(new WaitHitStunBehavior(){hitStunFlag = "KnockBack",Error = WaitTypeError.Success});
                        break;
                    }
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}