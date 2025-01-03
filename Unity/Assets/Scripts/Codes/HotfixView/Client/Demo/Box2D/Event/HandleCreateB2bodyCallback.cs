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
            BodyDef bodyDef = new()
            {
                BodyType = BodyType.DynamicBody,
                GravityScale = 0f,
                LinearDamping = 0f,
                AngularDamping = 0f,
                AllowSleep = true,
                FixedRotation = true,
            };
            b2WorldManager.Instance.CreateBody(args.instanceId, bodyDef);
        }
    }
}