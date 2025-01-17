using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerStayType.CollisionEvent)]
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(b2Body))]
    public class TriggerStay_HandleCollisionEvent : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            
            //碰撞缓冲区缓冲碰撞信息
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            b2Unit.CollisionBuffer.Enqueue(info);
        }
    }
}