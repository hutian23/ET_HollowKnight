using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    [FriendOf(typeof (b2Unit))]
    public class HandleUpdateFlipCallback: AInvokeHandler<UpdateFlipCallback>
    {
        public override void Handle(UpdateFlipCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();

            //1. Set Flip
            b2Body b2Body = b2WorldManager.Instance.GetBody(args.instanceId);
            b2Body.Flip = args.curFlip;
            //1-1 go sync flipState
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            go.transform.localScale = new Vector3( b2Body.GetFlip(), 1, 1);
            
            //2. Dispose old hitBoxFixtures
            b2Body.ClearHitbox();
            
            //3. FixedUpdate hitBoxFixtures
            foreach (BoxInfo info in b2Unit.keyFrame.boxInfos)
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
                b2Body.CreateHitbox(fixtureDef);
            }
        }
    }
}