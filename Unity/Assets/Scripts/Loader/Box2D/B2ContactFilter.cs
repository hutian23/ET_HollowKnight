using Box2DSharp.Dynamics;
using Timeline;

namespace ET
{
    public class B2ContactFilter: IContactFilter
    {
        public bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureA.UserData is not FixtureData dataA || fixtureB.UserData is not FixtureData dataB)
            {
                return false;
            }

            if (fixtureA.Body == fixtureB.Body)
            {
                return false;
            }

            if (dataA.UserData is BoxInfo infoA && dataB.UserData is BoxInfo infoB)
            {
                // Squash box不会相互碰撞
                if (infoA.hitboxType is HitboxType.Squash && infoB.hitboxType is HitboxType.Squash)
                {
                    return false;
                }
            }

            return true;
        }
    }
}