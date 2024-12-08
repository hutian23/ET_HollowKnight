using UnityEngine;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BehaviorInfo))]
    public class HandleUpdateProfileCallback : AInvokeHandler<UpdateUnitProfileCallback>
    {
        public override void Handle(UpdateUnitProfileCallback args)
        {
            if (args.instanceId == 0) return;

            //not found entity,Error
            b2Game b2Game = Camera.main.GetComponent<b2Game>();
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null)
            {
                return;
            }

            //Find Component
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body b2body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetCurrentOrder() == -1? null : buffer.GetInfoByOrder(buffer.GetCurrentOrder());

            b2Game.Profile = new UnitProfile()
            {
                UnitName = unit.GetComponent<GameObjectComponent>().GameObject.name,
                AngularVelocity = b2body.body.AngularVelocity,
                LinearVelocity = b2body.body.LinearVelocity,
                Position = b2body.body.GetPosition(),
                BehaviorName = info is null? string.Empty : info.behaviorName,
                MoveType = info is null? string.Empty : info.moveType.ToString()
            };
        }
    }
}