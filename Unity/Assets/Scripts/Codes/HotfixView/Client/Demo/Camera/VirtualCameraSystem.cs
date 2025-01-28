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

                if (parser.ContainParam("VC_DeadZone_Rect"))
                {
                    Rect rect = parser.GetParam<Rect>("VC_DeadZone_Rect");
                    
                    PolygonShape _shape = new();
                    _shape.SetAsBox(rect.size.x / 2f, rect.size.y / 2f, new Vector2(rect.center.x, rect.center.y), 0f);
                    b2WorldManager.Instance.DrawShape(_shape, Vector2.Zero, 0f, Color.Cyan);
                }
            }
        }
    }
}