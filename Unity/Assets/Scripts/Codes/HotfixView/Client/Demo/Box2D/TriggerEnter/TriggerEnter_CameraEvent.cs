using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerEnterType.CameraEvent)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(SceneBoxHandler))]
    public class TriggerEnter_CameraEvent : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            CollisionInfo info = args.info;
            if (!(info.dataB.LayerMask is LayerType.Camera))
            {
                return;
            }
            Log.Warning("Trigger Enter");
            
            b2Body body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(body.unitId) as Unit;
            SceneBoxHandler handler = unit.GetComponent<BBParser>().GetComponent<SceneBoxHandler>();
            handler.TriggerEnterQueue.Enqueue(info);
        }
    }
}