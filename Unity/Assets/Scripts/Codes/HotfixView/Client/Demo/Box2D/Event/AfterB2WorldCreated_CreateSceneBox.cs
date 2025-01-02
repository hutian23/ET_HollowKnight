using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Box2DSharp.Testbed.Unity.Inspection;
using ET.Event;
using Timeline;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(b2WorldManager))]
    public class AfterB2WorldCreate_CreateSceneBox : AEvent<AfterB2WorldCreated>
    {
        protected override async ETTask Run(Scene scene, AfterB2WorldCreated args)
        {
            World World = args.B2World.World;
            
            //not found sceneBox manager
            GameObject _World = GameObject.Find("_World");
            if (_World == null)
            {
                return;
            }
            
            //1. 场景内的static刚体当成一个部分
            Body sceneBody = World.CreateBody(new BodyDef() { BodyType = BodyType.StaticBody });
            
            //2. 建立映射
            b2Body b2Body = b2WorldManager.Instance.AddChild<b2Body>();
            b2Body.body = sceneBody;
            b2Body.unitId = 0;
            b2WorldManager.Instance.BodyDict.TryAdd(b2Body.unitId, b2Body.Id);
            
            //3. SceneBox作为sceneBody的Fixture
            foreach (b2Box sceneBox in _World.GetComponentsInChildren<b2Box>())
            {
                PolygonShape shape = new();
                shape.SetAsBox(sceneBox.info.size.x / 2, sceneBox.info.size.y / 2, sceneBox.info.center.ToVector2(), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0.0f,
                    UserData = new FixtureData()
                    {
                        InstanceId = 0, // 代表SceneBox
                        Name = sceneBox.info.boxName,
                        Type = FixtureType.Default,
                        LayerMask = LayerType.Ground,
                        IsTrigger = sceneBox.IsTrigger,
                        UserData = sceneBox.info
                    }
                };
                b2Body.CreateFixture(fixtureDef);
            }
            
            await ETTask.CompletedTask;
        }
    }
}