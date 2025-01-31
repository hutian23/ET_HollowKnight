using Box2DSharp.Collision.Shapes;
using UnityEngine;
using Color = Box2DSharp.Common.Color;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    public static class VirtualCameraSystem
    {
        public class VirtualCameraGizmosUpdateSystem : GizmosUpdateSystem<VirtualCamera>
        {
            protected override void GizmosUpdate(VirtualCamera self)
            {
                //1. confiner
                BBParser parser = self.GetParent<BBParser>();
                if (parser.ContainParam("VC_Confiner_Rect"))
                {
                    Rect rect = parser.GetParam<Rect>("VC_Confiner_Rect");
                    
                    PolygonShape _shape = new();
                    _shape.SetAsBox(rect.size.x / 2f, rect.size.y / 2f, new Vector2(rect.center.x, rect.center.y), 0f);
                    b2WorldManager.Instance.DrawShape(_shape, Vector2.Zero, 0f, Color.Cyan);
                }
                
                //DeadZone
                Rect DZ_rect = parser.GetParam<Rect>("VC_DeadZone_Rect");
                PolygonShape shape = new();
                shape.SetAsBox(DZ_rect.size.x / 2, DZ_rect.size.y / 2, new Vector2(DZ_rect.center.x, DZ_rect.center.y), 0f);
                b2WorldManager.Instance.DrawShape(shape, Vector2.Zero, 0f, Color.Red);
                
                //SoftZone
                Rect SZ_Rect = parser.GetParam<Rect>("VC_SoftZone_Rect");
                PolygonShape shape2 = new();
                shape2.SetAsBox(SZ_Rect.size.x / 2f, SZ_Rect.size.y / 2f, new Vector2(SZ_Rect.center.x, SZ_Rect.center.y), 0f);
                b2WorldManager.Instance.DrawShape(shape2, Vector2.Zero, 0f, Color.Cyan);
                
                // Draw CenterLine
                Vector3 min = Camera.main.ScreenToWorldPoint(Vector3.zero);
                float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
                float halfHeight = Camera.main.orthographicSize;
                b2WorldManager.Instance.DrawSegment(new Vector2(min.x + halfWidth,min.y), new Vector2(min.x + halfWidth,min.y + 2 * halfHeight), Color.White);
                
                // Follow Center
                UnityEngine.Vector2 center = parser.GetParam<UnityEngine.Vector2>("VC_Follow_Center");
                b2WorldManager.Instance.DrawPoint(center.ToVector2(),8f, Color.Blue);
                
                //Follow Target
                UnityEngine.Vector2 target = parser.GetParam<UnityEngine.Vector2>("VC_Follow_TargetPosition");
                b2WorldManager.Instance.DrawPoint(target.ToVector2(), 8f, Color.Yellow);
            }
        }
    }
}