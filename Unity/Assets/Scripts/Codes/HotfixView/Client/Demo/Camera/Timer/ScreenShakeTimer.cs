using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ScreenShakeTimer)]
    public class ScreenShakeTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            float shakeLength_X = self.GetParam<float>("ScreenShake_LengthX");
            float shakeLength_Y = self.GetParam<float>("ScreenShake_LengthY");
            float frequency = self.GetParam<float>("ScreenShake_Frequency");
            float totalFrame = self.GetParam<int>("ScreenShake_TotalFrame");
            float curFrame = self.GetParam<int>("ScreenShake_CurFrame");
            long timer = self.GetParam<long>("ScreenShake_Timer");
            GameObject go = self.GetParam<GameObject>("ScreenShake_ActiveCamera");
            CinemachineCameraOffset cameraOffset = go.GetComponent<CinemachineCameraOffset>();
                    
            // cameraOffset.m_Offset = new Vector3(shakeLength_X * Mathf.Cos(curFrame * frequency) * (curFrame / totalFrame), shakeLength_Y * Mathf.Sin(curFrame * frequency) * (curFrame / totalFrame), 0);
            cameraOffset.m_Offset = new Vector3(shakeLength_X * Mathf.Cos(curFrame * frequency) * (curFrame / totalFrame), shakeLength_Y * Mathf.Sin(curFrame * frequency) * 1, 0);
            
            curFrame--;
            if (curFrame <= 0)
            {
                lateUpdateTimer.Remove(ref timer);
                self.TryRemoveParam("ScreenShake_Timer");
                self.TryRemoveParam("ScreenShake_LengthX");
                self.TryRemoveParam("ScreenShake_LengthY");
                self.TryRemoveParam("ScreenShake_Frequency");
                self.TryRemoveParam("ScreenShake_TotalFrame");
                self.TryRemoveParam("ScreenShake_CurFrame");
                self.TryRemoveParam("ScreenShake_Timer");
                
                cameraOffset.m_Offset = Vector3.zero;
                self.TryRemoveParam("ScreenShake_ActiveCamera");
                return;
            }
            
            self.UpdateParam("ScreenShake_CurFrame", (int)curFrame);
        }
    }
}