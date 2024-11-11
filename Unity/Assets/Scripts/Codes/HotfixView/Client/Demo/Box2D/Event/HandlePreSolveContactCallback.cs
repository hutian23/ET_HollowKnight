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
                EventSystem.Instance.Invoke(dataA.TriggerStayId,new TriggerStayCallback(){fixtureA = fixtureA,dataA = dataA,fixtureB = fixtureB,dataB = dataB,Contact = args.contact});
            }

            if (dataB.TriggerStayId != 0)
            {
                EventSystem.Instance.Invoke(dataB.TriggerStayId,new TriggerStayCallback() { fixtureA = fixtureB,dataA = dataB,fixtureB = fixtureA,dataB = dataA,Contact = args.contact});
            }
        }
    }
}