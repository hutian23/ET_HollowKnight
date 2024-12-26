using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Box2DSharp.Testbed.Unity.Inspection;
using ET.Event;
using Timeline;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof (b2Body))]
    public class AfterB2WorldCreate_CreateSceneBox: AEvent<AfterB2WorldCreated>
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
            
            //create scene box
            foreach (SceneBox sceneBox in _World.GetComponentsInChildren<SceneBox>())
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
                        LayerMask = LayerType.Ground,
                        UserData = sceneBox.info,
                        IsTrigger = sceneBox.IsTrigger
                    }
                };
                //create body
                Body sceneBody = World.CreateBody(new BodyDef(){BodyType = BodyType.StaticBody});
                sceneBody.CreateFixture(fixtureDef);
            }
            await ETTask.CompletedTask;
        }
    }
}