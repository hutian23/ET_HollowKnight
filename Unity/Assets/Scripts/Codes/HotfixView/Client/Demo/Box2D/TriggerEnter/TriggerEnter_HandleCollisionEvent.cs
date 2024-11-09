using ET.Event;
using MongoDB.Bson;

namespace ET.Client
{
    [Invoke(TriggerEnterType.CollisionEvent)]
    public class TriggerEnter_HandleCollisionEvent: AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            Log.Warning(args.dataA.ToJson()+ "  "+args.dataB.ToJson());
        }
    }
}