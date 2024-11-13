using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (HitboxComponent))]
    [FriendOf(typeof (b2Body))]
    public class HandleUpdateHitBoxCallback: AInvokeHandler<UpdateHitboxCallback>
    {
        public override void Handle(UpdateHitboxCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            HitboxComponent hitBoxComponent = timelineComponent.GetComponent<HitboxComponent>();
            b2Body b2Body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            
            //更新关键帧
            hitBoxComponent.keyFrame = args.Keyframe; 

            //1. Dispose old hitBoxFixtures
            for (int i = 0; i < b2Body.hitBoxFixtures.Count; i++)
            {
                Fixture fixture = b2Body.hitBoxFixtures[i];
                b2Body.body.DestroyFixture(fixture);
            }  
            b2Body.hitBoxFixtures.Clear();
            
            //2. update hitBoxFixtures
            foreach (BoxInfo info in args.Keyframe.boxInfos)
            {
                PolygonShape shape = new();
                shape.SetAsBox(info.size.x / 2, info.size.y / 2, new Vector2(info.center.x * b2Body.GetFlip(), info.center.y), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0f,
                    UserData = new FixtureData()
                    {
                        InstanceId = b2Body.InstanceId, 
                        LayerMask = LayerType.Unit, 
                        IsTrigger = info.hitboxType is not HitboxType.Squash,
                        UserData = info,
                        TriggerEnterId = TriggerEnterType.CollisionEvent,
                        TriggerStayId = TriggerStayType.CollisionEvent,
                        TriggerExitId = TriggerExitType.CollisionEvent
                    }
                };
                Fixture fixture = b2Body.body.CreateFixture(fixtureDef);
                b2Body.hitBoxFixtures.Add(fixture);
            }
        }
    }
}