using System;
using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraFOVTimer)]
    [FriendOf(typeof(VirtualCamera))]
    [FriendOf(typeof(CameraTarget))]
    public class CameraFOVTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            Vector2 center = self.GetParam<Rect>("VC_DeadZone_Rect").center;
            // float curFov = self.GetParam<float>("VC_CurrentFOV");
            float minFov = self.GetParam<float>("VC_MinFOV");
            float maxFov = self.GetParam<float>("VC_MaxFOV");
            
            VirtualCamera vc = self.GetComponent<VirtualCamera>();
            float halfWidth = 0f, halfHeight = 0f;
            
            foreach (var kv in vc.targetIds)
            {
                CameraTarget target = vc.GetChild<CameraTarget>(kv.Value);
                Unit unit = Root.Instance.Get(target.UnitId) as Unit;
                if (unit == null)
                {
                    continue;
                }
                GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
                
                halfWidth = Math.Max(halfWidth, Math.Abs(go.transform.position.x - center.x));
                halfHeight = Math.Max(halfHeight, Math.Abs(go.transform.position.y - center.y));
            }
            float fov = Math.Max(halfHeight, halfWidth * Screen.height / Screen.width);
            float _curFov = Math.Clamp(fov, minFov, maxFov);
            
            Camera.main.orthographicSize = _curFov;
        }
    }
}