using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleBeginContactCallback : AInvokeHandler<BeginContact>
    {
        public override void Handle(BeginContact args)
        {
            Fixture fixtureA = args.contact.FixtureA;
            Fixture fixtureB = args.contact.FixtureB;
            
            if (fixtureA.UserData is not FixtureData dataA || fixtureB.UserData is not FixtureData dataB)
            {
                return;
            }

            if (dataA.TriggerEnterId != 0)
            {
                EventSystem.Instance.Invoke(dataA.TriggerEnterId, new TriggerEnterCallback() { fixtureA = fixtureA,dataA = dataA, fixtureB = fixtureB, dataB = dataB,Contact = args.contact});
            }

            if (dataB.TriggerEnterId != 0)
            {
                EventSystem.Instance.Invoke(dataB.TriggerEnterId,new TriggerEnterCallback(){fixtureA = fixtureB, dataA = dataB, fixtureB = fixtureA, dataB =  dataA,Contact = args.contact});
            }
        }
    }
}