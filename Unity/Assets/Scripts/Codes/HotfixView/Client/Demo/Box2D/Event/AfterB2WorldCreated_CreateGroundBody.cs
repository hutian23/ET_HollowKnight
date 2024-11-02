using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof (b2Body))]
    public class AfterB2WorldCreate_CreateGroundBody: AEvent<AfterB2WorldCreated>
    {
        protected override async ETTask Run(Scene scene, AfterB2WorldCreated args)
        {
            World World = args.B2World.World;

            //ground
            BodyDef groundBodyDef = new()
            {
                BodyType = BodyType.StaticBody, 
                Position = Vector2.Zero
            };
            Body groundBody = World.CreateBody(groundBodyDef);
            PolygonShape groundBox = new();
            groundBox.SetAsBox(50f, 2.0f);
            Fixture fixture =  groundBody.CreateFixture(groundBox, 0.0f);
            fixture.UserData = new FixtureData() { InstanceId = 0, LayerMask = LayerType.Ground };
            
            //obstacle
            // var obstacleDef = new BodyDef() { BodyType = BodyType.StaticBody, Position = new Vector2(-5, 4f) };
            // var obstacleBody = World.CreateBody(obstacleDef);
            // var box = new PolygonShape();
            // box.SetAsBox(1f, 1f);
            // obstacleBody.CreateFixture(box, 0.0f);

            await ETTask.CompletedTask;
        }
    }
}