using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraOffsetMoveTimer)]
    public class FollowOffsetDampingTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            float targetOffset = self.GetParam<float>("VC_Follow_Offset") * self.GetParam<int>("VC_Follow_OffsetFlip");
            float preOffset = self.GetParam<float>("VC_Follow_CurrentOffset");
            float dampingX = self.GetParam<float>("VC_Follow_Damping");
            float curOffset = Mathf.Lerp(preOffset, targetOffset, dampingX / 60f);

            self.UpdateParam("VC_Follow_CurrentOffset", curOffset);
            if (Mathf.Abs(curOffset - targetOffset) < 0.01f)
            {
                BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
                long timer = self.GetParam<long>("VC_Follow_OffsetDampingTimer");
                lateUpdateTimer.Remove(ref timer);

                self.TryRemoveParam("VC_Follow_OffsetDampingTimer");
                self.TryRemoveParam("VC_Follow_OffsetFlip");
            }
        }
    }
}