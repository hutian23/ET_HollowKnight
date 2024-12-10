using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(b2WorldManager))]    
    //这个事件用于建立Unit和b2world中刚体的映射
    public class HandleCreateB2bodyCallback : AInvokeHandler<CreateB2bodyCallback>
    {
        public override void Handle(CreateB2bodyCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            World world = b2WorldManager.Instance.B2World.World;

            //创建刚体
            BodyDef bodyDef = new()
            {
                BodyType = BodyType.DynamicBody,
                GravityScale = 0f,
                LinearDamping = 0f,
                AngularDamping = 0f,
                AllowSleep = true,
                FixedRotation = true,
            };
            Body body = world.CreateBody(bodyDef);

            b2Body b2Body = b2WorldManager.Instance.AddChild<b2Body>();
            b2Body.body = body;
            b2Body.unitId = timelineComponent.GetParent<Unit>().InstanceId;
            b2WorldManager.Instance.BodyDict.TryAdd(b2Body.unitId, b2Body.Id);
        }
    }
}