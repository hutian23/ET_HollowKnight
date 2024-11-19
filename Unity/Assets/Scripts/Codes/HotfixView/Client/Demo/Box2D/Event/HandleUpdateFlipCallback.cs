using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    [FriendOf(typeof (HitboxComponent))]
    public class HandleUpdateFlipCallback: AInvokeHandler<UpdateFlipCallback>
    {
        public override void Handle(UpdateFlipCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitBoxComponent = timelineComponent.GetComponent<HitboxComponent>();

            //1. Set Flip
            b2Body b2Body = b2GameManager.Instance.GetBody(args.instanceId);
            b2Body.Flip = args.curFlip;
            //1-1 go sync flipState
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            go.transform.localScale = new Vector3( b2Body.GetFlip(), 1, 1);
            
            //2. Dispose old hitBoxFixtures
            for (int i = 0; i < b2Body.hitBoxFixtures.Count; i++)
            {
                Fixture fixture = b2Body.hitBoxFixtures[i];
                b2Body.body.DestroyFixture(fixture);
            }
            b2Body.hitBoxFixtures.Clear();
            
            //3. FixedUpdate hitBoxFixtures
            foreach (BoxInfo info in hitBoxComponent.keyFrame.boxInfos)
            {
                PolygonShape shape = new();
                shape.SetAsBox(info.size.x / 2, info.size.y / 2, new Vector2(info.center.x * b2Body.GetFlip(), info.center.y), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0.0f,
                    UserData = new FixtureData()
                    {
                        InstanceId = b2Body.InstanceId, 
                        LayerMask = LayerType.Unit, 
                        IsTrigger = info.hitboxType is not HitboxType.Squash,
                        UserData = info,
                        TriggerStayId = TriggerStayType.CollisionEvent,
                    },
                };
                Fixture fixture = b2Body.body.CreateFixture(fixtureDef);
                b2Body.hitBoxFixtures.Add(fixture);
            }
        }
    }
}