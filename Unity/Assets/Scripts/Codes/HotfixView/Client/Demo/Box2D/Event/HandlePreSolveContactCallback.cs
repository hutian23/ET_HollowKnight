using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandlePreSolveContactCallback : AInvokeHandler<PreSolveContact>
    {
        public override void Handle(PreSolveContact args)
        {
            Fixture fixtureA = args.contact.FixtureA;
            Fixture fixtureB = args.contact.FixtureB;

            if (fixtureA.UserData is not FixtureData dataA || fixtureB.UserData is not FixtureData dataB)
            {
                return;
            }

            if (dataA.TriggerStayId != 0)
            {
                EventSystem.Instance.Invoke(dataA.TriggerStayId,new TriggerStayCallback()
                {
                    info = new CollisionInfo()
                    {
                        fixtureA = fixtureA,
                        fixtureB =  fixtureB,
                        dataA = dataA,
                        dataB = dataB,
                        Contact = args.contact
                    }
                });
            }

            if (dataB.TriggerStayId != 0)
            {
                EventSystem.Instance.Invoke(dataB.TriggerStayId,new TriggerStayCallback()
                {
                    info =  new CollisionInfo()
                    {
                        fixtureA = fixtureB,
                        fixtureB = fixtureA,
                        dataA = dataB,
                        dataB = dataA,
                        Contact = args.contact
                    }
                });
            }
        }
    }
}