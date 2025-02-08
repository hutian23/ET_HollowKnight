using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CinemachineTargetTimer)]
    public class CinemachineTargetTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            GameObject go = self.GetParam<GameObject>("CM_Follow_Go");
            GameObject target = self.GetParam<GameObject>("CM_CameraTarget");
            
            // 初始化
            if (go == null)
            {
                BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
                long timer = self.GetParam<long>("CM_Follow_Timer");
                lateUpdateTimer.Remove(ref timer);
                
                self.TryRemoveParam("CM_Follow_Timer");
                self.TryRemoveParam("CM_Follow_Go");
                return;
            }
            
            // 同步位置
            target.transform.position = go.transform.position;
        }
    }
}