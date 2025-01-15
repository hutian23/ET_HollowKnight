using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionExitType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    [FriendOf(typeof(b2Body))]
    public class CollisionExit_SceneBoxEvent : AInvokeHandler<CollisionExitCallback>
    {
        public override void Handle(CollisionExitCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            SceneBoxHandler handler = unit.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.CollisionExitQueue.Enqueue(info);
        }
    }
}