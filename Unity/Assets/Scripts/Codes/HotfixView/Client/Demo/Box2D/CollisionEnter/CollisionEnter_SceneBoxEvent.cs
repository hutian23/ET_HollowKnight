using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionEnterType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    [FriendOf(typeof(b2Body))]
    public class CollisionEnter_SceneBoxEvent : AInvokeHandler<CollisionEnterCallback>
    {
        public override void Handle(CollisionEnterCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            SceneBoxHandler handler = unit.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.CollisionEnterQueue.Enqueue(info);
        }
    }
}