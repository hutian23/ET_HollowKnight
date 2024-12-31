using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class RootInit_RegistAirCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistAirCheck";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 注册变量
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            timelineComponent.RegistParam("InAir", false);

            //2. 创建夹具
            // PolygonShape shape = new();
            // shape.SetAsBox(0.4f, 0.6f, new Vector2(0, -2.0f), 0f);
            // FixtureDef fixtureDef = new()
            // {
            //     Shape = shape,
            //     Density = 0.0f,
            //     Friction = 0.0f,
            //     UserData = new FixtureData()
            //     {
            //         InstanceId = timelineComponent.InstanceId,
            //         LayerMask = LayerType.Unit,
            //         IsTrigger = true,
            //         UserData = new BoxInfo() { hitboxType = HitboxType.Gizmos },
            //         TriggerEnterId = TriggerEnterType.GroundCollision,
            //         TriggerExitId = TriggerExitType.GroundCollision
            //     },
            // };
            // b2Body B2body = b2WorldManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            // Fixture groundCheckBox = B2body.body.CreateFixture(fixtureDef);
            // timelineComponent.RegistParam("GroundCheckBox", groundCheckBox);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}