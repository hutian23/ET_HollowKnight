using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionEnterType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    public class CollisionEnter_SceneBoxEvent : AInvokeHandler<CollisionEnterCallback>
    {
        public override void Handle(CollisionEnterCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            SceneBoxHandler handler = b2Body.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.CollisionEnterQueue.Enqueue(info);
        }
    }
}