using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionStayType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    [FriendOf(typeof(b2Body))]
    public class CollisionStay_SceneBoxEvent : AInvokeHandler<CollisionStayCallback>
    {
        public override void Handle(CollisionStayCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            SceneBoxHandler handler = unit.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.CollisionStayQueue.Enqueue(info);
        }
    }
}