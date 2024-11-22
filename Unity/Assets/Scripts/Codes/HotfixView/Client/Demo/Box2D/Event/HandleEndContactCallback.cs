using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleEndContactCallback : AInvokeHandler<EndContactCallback>
    {
        public override void Handle(EndContactCallback args)
        {
            Fixture fixtureA = args.Contact.FixtureA;
            Fixture fixtureB = args.Contact.FixtureB;
            
            
            if (fixtureA == null || fixtureB == null || 
                fixtureA.UserData is not FixtureData dataA ||
                fixtureB.UserData is not FixtureData dataB)
            {
                return;
            }

            if (dataA.TriggerExitId != 0)
            {
                EventSystem.Instance.Invoke(dataA.TriggerExitId,new TriggerExitCallback()
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

            if (dataB.TriggerExitId != 0)
            {
                EventSystem.Instance.Invoke(dataB.TriggerExitId,new TriggerExitCallback()
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