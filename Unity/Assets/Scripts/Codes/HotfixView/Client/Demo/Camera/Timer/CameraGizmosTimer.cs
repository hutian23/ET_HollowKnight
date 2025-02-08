using Box2DSharp.Collision.Shapes;
using UnityEngine;
using Color = Box2DSharp.Common.Color;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraGizmosTimer)]
    public class CameraGizmosTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser parser)
        {
             // //1. DeadZone
             // Rect DZ_rect = parser.GetParam<Rect>("VC_DeadZone_Rect");
             // PolygonShape shape = new();
             // shape.SetAsBox(DZ_rect.size.x / 2f, DZ_rect.size.y / 2f, new Vector2(DZ_rect.center.x, DZ_rect.center.y), 0f);
             // b2WorldManager.Instance.DrawShape(shape, Vector2.Zero, 0f, Color.Red);
             //
             // //2. SoftZone
             // Rect SZ_Rect = parser.GetParam<Rect>("VC_SoftZone_Rect");
             // PolygonShape shape2 = new();
             // shape2.SetAsBox(SZ_Rect.size.x / 2f, SZ_Rect.size.y / 2f, new Vector2(SZ_Rect.center.x, SZ_Rect.center.y), 0f);
             // b2WorldManager.Instance.DrawShape(shape2, Vector2.Zero, 0f, Color.Cyan);
             //
             // //3. Draw CenterLine
             // Vector3 min = Camera.main.ScreenToWorldPoint(Vector3.zero);
             // float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
             // float halfHeight = Camera.main.orthographicSize;
             // b2WorldManager.Instance.DrawSegment(new Vector2(min.x + halfWidth, min.y), new Vector2(min.x + halfWidth, min.y + 2 * halfHeight), Color.White);
             // b2WorldManager.Instance.DrawSegment(new Vector2(min.x, min.y + halfHeight), new Vector2(min.x + halfWidth * 2, min.y + halfHeight), Color.White);
             //
             // //3. Confiner Extension
             // if (parser.ContainParam("VC_Confiner_Rect"))
             // {
             //     Rect rect = parser.GetParam<Rect>("VC_Confiner_Rect");
             //
             //     PolygonShape _shape = new();
             //     _shape.SetAsBox(rect.size.x / 2f, rect.size.y / 2f, new Vector2(rect.center.x, rect.center.y), 0f);
             //     b2WorldManager.Instance.DrawShape(_shape, Vector2.Zero, 0f, Color.Cyan);
             // }
             //
             // //4. Follow Extension
             // if (parser.ContainParam("VC_Follow_Center"))
             // {
             //     UnityEngine.Vector2 center = parser.GetParam<UnityEngine.Vector2>("VC_Follow_Center");
             //     b2WorldManager.Instance.DrawPoint(center.ToVector2(), 8f, Color.Blue);
             // }
             // if (parser.ContainParam("VC_Follow_TargetPosition"))
             // {
             //     UnityEngine.Vector2 target = parser.GetParam<UnityEngine.Vector2>("VC_Follow_TargetPosition");
             //     b2WorldManager.Instance.DrawPoint(target.ToVector2(), 8f, Color.Yellow);
             // }
             //
             // //6. TargetGroup Extension
             // if (parser.ContainParam("VC_TargetGroup"))
             // {
             //     ListComponent<CameraTarget> targetGroups = parser.GetParam<ListComponent<CameraTarget>>("VC_TargetGroup");
             //     foreach (CameraTarget cameraTarget in targetGroups)
             //     {
             //         b2Body body = b2WorldManager.Instance.GetBody(cameraTarget.instanceId);
             //         b2WorldManager.Instance.DrawPoint(body.GetPosition(), 9f, Color.Blue);
             //     }   
             // }

             if (parser.ContainParam("CM_CameraTarget"))
             {
                 GameObject target = parser.GetParam<GameObject>("CM_CameraTarget");
                 b2WorldManager.Instance.DrawPoint(target.transform.position.ToVector2(), 9f, Color.Blue);
             }
        }
    }
}