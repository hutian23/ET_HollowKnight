using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerStayType.CameraEvent)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(SceneBoxHandler))]
    public class TriggerStay_CameraEvent : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            CollisionInfo info = args.info;
            if (!(info.dataB.LayerMask is LayerType.Camera))
            {
                return;
            }

            b2Body body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(body.unitId) as Unit;
            SceneBoxHandler handler = unit.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.TriggerStayQueue.Enqueue(info);
        }
    }
}