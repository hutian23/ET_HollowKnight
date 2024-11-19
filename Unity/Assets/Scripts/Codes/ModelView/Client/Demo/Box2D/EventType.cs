using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Timeline;

namespace ET.Event
{
    public struct AfterB2WorldCreated
    {
        public b2World B2World;
    }

    #region 主要是区分回调
    public struct TriggerEnterCallback
    {
        public CollisionInfo info;
    }
    public struct TriggerExitCallback
    {
        public CollisionInfo info;
    }
    public struct TriggerStayCallback
    {
        public CollisionInfo info;
    }
    #endregion
    
    public struct CollisionInfo
    {
        //碰撞事件调用者
        public Fixture fixtureA;
        public FixtureData dataA;
        //和谁发生碰撞
        public Fixture fixtureB;
        public FixtureData dataB;
        //接触点
        public Contact Contact;
    }
}