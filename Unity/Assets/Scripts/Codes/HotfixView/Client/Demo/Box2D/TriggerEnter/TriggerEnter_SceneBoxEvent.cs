using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerEnterType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    [FriendOf(typeof(b2Body))]
    public class TriggerEnter_SceneBoxEvent : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            SceneBoxHandler handler = unit.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.TriggerEnterQueue.Enqueue(info);
        }
    }
}