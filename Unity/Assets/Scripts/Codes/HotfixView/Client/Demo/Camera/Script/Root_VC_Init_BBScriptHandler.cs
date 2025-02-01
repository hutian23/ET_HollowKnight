using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class Root_VC_Init_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Init";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            parser.RemoveComponent<VirtualCamera>();
            VirtualCamera vc = parser.AddComponent<VirtualCamera>();
            CameraManager.instance.vc_InstanceId = vc.InstanceId;

            //1. Camera Follow
            long followTimer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraFollowTimer, parser);
            parser.RegistParam("VC_Follow_Id", 0);
            parser.RegistParam("VC_Follow_TargetPosition", Vector2.zero);
            parser.RegistParam("VC_Follow_Center", Vector2.zero);
            parser.RegistParam("VC_Follow_Flip", -1);
            parser.RegistParam("VC_Follow_Delay", 5);
            parser.RegistParam("VC_Follow_Offset", 0f);
            parser.RegistParam("VC_Follow_CurrentOffset", 0f);
            parser.RegistParam("VC_Follow_Damping", 10f);
            parser.RegistParam("VC_Follow_Timer", followTimer);
            token.Add(() =>
            {
                lateUpdateTimer.Remove(ref followTimer);
            });

            //2. Zone
            long zoneTimer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraZoneTimer, parser);
            parser.RegistParam("VC_DeadZone_X", 0f);
            parser.RegistParam("VC_DeadZone_Y", 0f);
            parser.RegistParam("VC_SoftZone_X", 1f);
            parser.RegistParam("VC_SoftZone_Y", 1f);
            parser.RegistParam("VC_SoftZone_Rect", new Rect());
            parser.RegistParam("VC_DeadZone_Rect", new Rect());
            parser.RegistParam("VC_Bias_X", 0.01f);
            parser.RegistParam("VC_Bias_Y", 0.01f);
            parser.RegistParam("VC_ZoneTimer", zoneTimer);
            token.Add(() =>
            {
                lateUpdateTimer.Remove(ref zoneTimer);
            });

            //3. FOV
            long fovTimer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraFOVTimer, parser);
            parser.RegistParam("VC_CurrentFOV", 8f);
            parser.RegistParam("VC_MinFOV", 8f);
            parser.RegistParam("VC_MaxFOV", 8f);
            parser.RegistParam("VC_FOVTimer", fovTimer);
            token.Add(() =>
            {
                lateUpdateTimer.Remove(ref fovTimer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}