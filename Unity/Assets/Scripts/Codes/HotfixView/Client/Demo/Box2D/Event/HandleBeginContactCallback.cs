using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleBeginContactCallback : AInvokeHandler<BeginContactCallback>
    {
        public override void Handle(BeginContactCallback args)
        {
            Fixture fixtureA = args.Contact.FixtureA;
            Fixture fixtureB = args.Contact.FixtureB;

            if (fixtureA == null || fixtureB == null || 
                fixtureA.UserData is not FixtureData dataA ||
                fixtureB.UserData is not FixtureData dataB)
            {
                return;
            }

            if (dataA.TriggerEnterId != 0)
            {
                EventSystem.Instance.Invoke(dataA.TriggerEnterId,new TriggerEnterCallback()
                {
                    info = new CollisionInfo()
                    {
                        fixtureA = fixtureA,
                        fixtureB =  fixtureB,
                        dataA = dataA,
                        dataB = dataB,
                        Contact = args.Contact
                    }
                });
            }

            if (dataB.TriggerEnterId != 0)
            {
                EventSystem.Instance.Invoke(dataB.TriggerEnterId,new TriggerEnterCallback()
                {
                    info =  new CollisionInfo()
                    {
                        fixtureA = fixtureB,
                        fixtureB = fixtureA,
                        dataA = dataB,
                        dataB = dataA,
                        Contact = args.Contact
                    }
                });
            }
        }
    }
}