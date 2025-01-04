using ET.Event;

namespace ET.Client.TriggerExit
{
    [Invoke(TriggerExitType.SceneBoxEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    public class TriggerExit_SceneBoxEvent : AInvokeHandler<TriggerExitCallback>
    {
        public override void Handle(TriggerExitCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            SceneBoxHandler handler = b2Body.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.TriggerExitQueue.Enqueue(info);
        }
    }
}