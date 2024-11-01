using Box2DSharp.Dynamics;
using MongoDB.Bson;

namespace ET.Client
{
    [Invoke]
    public class HandleBeginContactCallback : AInvokeHandler<BeginContact>
    {
        public override void Handle(BeginContact args)
        {
            Fixture fixtureA = args.contact.FixtureA;
            Fixture fixtureB = args.contact.FixtureB;
            Log.Warning(fixtureA.UserData.ToJson()+"  "+ fixtureB.UserData.ToJson());
        }
    }
}