using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerExitType.CameraEvent)]
    [FriendOf(typeof(SceneBoxHandler))]
    [FriendOf(typeof(b2Body))]
    public class TriggerExit_CameraEvent : AInvokeHandler<TriggerExitCallback>
    {
        public override void Handle(TriggerExitCallback args)
        {
            CollisionInfo info = args.info;
            if (!(info.dataB.LayerMask is LayerType.Camera))
            {
                return;
            }

            b2Body body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(body.unitId) as Unit;
            SceneBoxHandler handler = unit.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.TriggerExitQueue.Enqueue(info);
        }
    }
}