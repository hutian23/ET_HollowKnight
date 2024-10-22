using Box2DSharp.Dynamics;
using Timeline;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    public class GroundCheckRayCastCallback: IRayCastCallback
    {
        public bool Hit;
        
        public float RayCastCallback(Fixture fixture, in Vector2 point, in Vector2 normal, float fraction)
        {
            var body = fixture.Body;
            if (body.UserData is not FixtureData data) return -1.0f;
            if ((data.LayerMask & LayerType.Ground) != 0)
            {
                Hit = true;
                return 0f;
            }
            
            return -1.0f;
        }

        public static GroundCheckRayCastCallback Create()
        {
            return ObjectPool.Instance.Fetch<GroundCheckRayCastCallback>();
        }

        public void Recycle()
        {
            Hit = false;
            ObjectPool.Instance.Recycle(this);
        }
    }
}