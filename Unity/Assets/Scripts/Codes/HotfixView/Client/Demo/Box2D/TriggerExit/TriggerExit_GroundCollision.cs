using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerExitType.GroundCollision)]
    public class TriggerExit_GroundCollision : AInvokeHandler<TriggerExitCallback>
    {
        public override void Handle(TriggerExitCallback args)
        {
            CollisionInfo info = args.info;

            if (info.dataB.LayerMask != LayerType.Ground)
            {
                return;
            }
            
            TimelineComponent timelineComponent = Root.Instance.Get(info.dataA.InstanceId) as TimelineComponent;
            timelineComponent.UpdateParam("InAir", true);
            
            //离地直接完成充能
            if (timelineComponent.ContainParam("DashRechargeToken"))
            {
                ETCancellationToken rechargeToken = timelineComponent.GetParam<ETCancellationToken>("DashRechargeToken");
                rechargeToken.Cancel();
                timelineComponent.TryRemoveParam("DashRechargeToken");
            }
            int maxDash = (int)timelineComponent.GetParam<long>("MaxDash");
            timelineComponent.UpdateParam("DashCount", maxDash);
        }
    }
}