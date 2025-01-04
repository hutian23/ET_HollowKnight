using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionStayType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    public class CollisionStay_SceneBoxEvent : AInvokeHandler<CollisionStayCallback>
    {
        public override void Handle(CollisionStayCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            SceneBoxHandler handler = b2Body.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.CollisionStayQueue.Enqueue(info);
        }
    }
}