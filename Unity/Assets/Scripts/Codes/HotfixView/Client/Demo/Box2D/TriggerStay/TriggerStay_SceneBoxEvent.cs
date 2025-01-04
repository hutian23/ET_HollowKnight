using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerStayType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    public class TriggerStay_SceneBoxEvent : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            SceneBoxHandler handler = b2Body.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.TriggerStayQueue.Enqueue(info);
        }
    }
}