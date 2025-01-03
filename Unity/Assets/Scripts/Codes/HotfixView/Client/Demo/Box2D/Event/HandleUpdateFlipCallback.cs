using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    public class HandleUpdateFlipCallback: AInvokeHandler<UpdateFlipCallback>
    {
        public override void Handle(UpdateFlipCallback args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            b2Body b2Body = b2WorldManager.Instance.GetBody(args.instanceId);
            
            //1. 显示层更新朝向
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            go.transform.localScale = new Vector3( b2Body.GetFlip(), 1, 1);
            
            //2. 逻辑层
            QueueComponent<FixtureData> dataQueue = new QueueComponent<FixtureData>();
            foreach (Fixture fixture in b2Body.Fixtures)
            {
                dataQueue.Enqueue((FixtureData)fixture.UserData);
            }
            b2Body.ClearFixtures();
            
            int count = dataQueue.Count;
            while (count-- > 0)
            {
                FixtureData data = dataQueue.Dequeue();
                if (data.UserData is not BoxInfo info) continue;
                //reset param of fixtureDef
                PolygonShape shape = new();
                shape.SetAsBox(info.size.x / 2, info.size.y / 2, new Vector2(info.center.x * b2Body.GetFlip(), info.center.y), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0.0f,
                    UserData = data
                };
                b2Body.CreateFixture(fixtureDef);
            }
        }
    }
}