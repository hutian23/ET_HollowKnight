using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionExitType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    public class CollisionExit_SceneBoxEvent : AInvokeHandler<CollisionExitCallback>
    {
        public override void Handle(CollisionExitCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            SceneBoxHandler handler = b2Body.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.CollisionExitQueue.Enqueue(info);
        }
    }
}