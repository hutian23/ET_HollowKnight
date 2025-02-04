using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraFOVTransitionTimer)]
    public class CameraFOVTransitionTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            float damping = self.GetParam<float>("VC_FOV_Damping");
            float targetFOV = self.GetParam<float>("VC_FOV_TargetFOV");
            float preFOV = self.GetParam<float>("VC_CurrentFOV");
            
            float currentFOV = Mathf.Lerp(preFOV, targetFOV, damping / 60f);
            self.UpdateParam("VC_CurrentFOV", currentFOV);

            // 销毁定时器
            if (Mathf.Abs(targetFOV - currentFOV) < 0.01f)
            {
                self.TryRemoveParam("VC_FOV_Damping");
                self.TryRemoveParam("VC_FOV_TargetFOV");
                if (self.ContainParam("VC_FOV_TransitionTimer"))
                {
                    long timer = self.GetParam<long>("VC_FOV_TransitionTimer");
                    BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
                    lateUpdateTimer.Remove(ref timer);   
                }
                self.TryRemoveParam("VC_FOV_TransitionTimer");
            }
        }
    }
}