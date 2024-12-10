using System.Numerics;
using Box2DSharp.Testbed.Unity.Inspection;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    public class HandleUpdateRootMotionCallback: AInvokeHandler<UpdateRootMotionCallback>
    {
        public override void Handle(UpdateRootMotionCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body B2body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            if (!args.ApplyRootMotion)
            {
                return;
            }
            B2body.SetVelocity(args.velocity.ToVector2() * new Vector2(B2body.GetFlip(),1));
            Log.Warning(args.velocity+"  "+ "RootMotion"+"  "+B2body.GetVelocity());
        }
    }
}