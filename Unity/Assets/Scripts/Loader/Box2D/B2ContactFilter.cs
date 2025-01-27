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
                // 编辑器阶段绘制的图形，不参与碰撞
                if (infoA.hitboxType is HitboxType.Gizmos || infoB.hitboxType is HitboxType.Gizmos)
                {
                    return false;
                }
                // Unit之间不会相互碰撞
                if (infoA.hitboxType is HitboxType.Squash && infoB.hitboxType is HitboxType.Squash && 
                    dataA.LayerMask is LayerType.Unit && dataB.LayerMask is LayerType.Unit)
                {
                    return false;
                }
            }

            return true;
        }
    }
}