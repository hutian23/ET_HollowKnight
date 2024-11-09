using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Event
{
    public struct AfterB2WorldCreated
    {
        public b2World B2World;
    }

    public struct TriggerEnterCallback
    {
        //TriggerEnter回调的调用者
        public Fixture fixtureA;
        public FixtureData dataA;
        
        public Fixture fixtureB;
        public FixtureData dataB;
    }

    public struct TriggerExitCallback
    {
        public Fixture fixtureA;
        public FixtureData dataA;

        public Fixture fixtureB;
        public FixtureData dataB;
    }

    public struct TriggerStayCallback
    {
        public Fixture fixtureA;
        public FixtureData dataA;

        public Fixture fixtureB;
        public FixtureData dataB;
    }
}