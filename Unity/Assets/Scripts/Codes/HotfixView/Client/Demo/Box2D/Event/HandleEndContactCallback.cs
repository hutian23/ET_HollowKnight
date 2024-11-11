using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleEndContactCallback : AInvokeHandler<EndContact>
    {
        public override void Handle(EndContact args)
        {
            Fixture fixtureA = args.contact.FixtureA;
            Fixture fixtureB = args.contact.FixtureB;
            
            if (fixtureA.UserData is not FixtureData dataA || fixtureB.UserData is not FixtureData dataB)
            {
                return;
            }

            if (dataA.TriggerExitId != 0)
            {
                EventSystem.Instance.Invoke(dataA.TriggerExitId, new TriggerExitCallback() { fixtureA = fixtureA,dataA = dataA, fixtureB = fixtureB, dataB = dataB,Contact = args.contact});
            }

            if (dataB.TriggerExitId != 0)
            {
                EventSystem.Instance.Invoke(dataB.TriggerExitId,new TriggerExitCallback(){fixtureA = fixtureB, dataA = dataB, fixtureB = fixtureA, dataB =  dataA,Contact = args.contact});
            }
        }
    }
}