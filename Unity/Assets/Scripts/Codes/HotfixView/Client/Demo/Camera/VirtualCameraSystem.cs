using Box2DSharp.Collision.Shapes;
using MongoDB.Bson;
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
                b2WorldManager.Instance.DrawShape(shape, Vector2.Zero, 0f, Color.Cyan);
                
                // //SoftZone
                // Rect SZ_Rect = parser.GetParam<Rect>("VC_SoftZone_Rect");
                // PolygonShape shape2 = new();
                // shape.SetAsBox(SZ_Rect.size.x / 2f, SZ_Rect.size.y / 2f, new Vector2(SZ_Rect.center.x, SZ_Rect.center.y), 0f);
                // b2WorldManager.Instance.DrawShape(shape2, Vector2.Zero, 0f, Color.Cyan);
            }
        }
    }
}