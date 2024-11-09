using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(b2Body))]
    public class ReloadTimelineComponent_CreateFixture : AEvent<ReloadTimelineComponent>
    {
        protected override async ETTask Run(Scene scene, ReloadTimelineComponent args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            b2Body B2body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);

            for (int i = 0; i < B2body.fixtures.Count; i++)
            {
                Fixture fixture = B2body.fixtures[i];
                B2body.body.DestroyFixture(fixture);
            }
            B2body.fixtures.Clear();
            
            //1. 地面检测
            PolygonShape shape = new();
            shape.SetAsBox(0.4f, 0.6f, new Vector2(0, -2.0f), 0f);
            FixtureDef fixtureDef = new()
            {
                Shape = shape,
                Density = 0.0f,
                Friction = 0.0f,
                UserData = new FixtureData()
                {
                    InstanceId = timelineComponent.InstanceId,
                    LayerMask = LayerType.Unit, 
                    IsTrigger = true,
                    UserData =  new BoxInfo(){hitboxType = HitboxType.Gizmos},
                    TriggerEnterId = TriggerEnterType.GroundCollision,
                    TriggerExitId = TriggerExitType.GroundCollision
                },
            };
            Fixture groundFixture = B2body.body.CreateFixture(fixtureDef);
            B2body.fixtures.Add(groundFixture);

            await ETTask.CompletedTask;
        }
    }
}