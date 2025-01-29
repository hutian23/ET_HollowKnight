using UnityEngine;

namespace ET.Client
{
    public class Root_VC_Init_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Init";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            parser.RemoveComponent<VirtualCamera>();
            parser.AddComponent<VirtualCamera>();
            
            //1. Zone
            parser.RegistParam("VC_DeadZone_X", 0f);
            parser.RegistParam("VC_DeadZone_Y", 0f);
            parser.RegistParam("VC_SoftZone_X", 1f);
            parser.RegistParam("VC_SoftZone_Y", 1f);
            parser.RegistParam("VC_Bias_X", 0f);
            parser.RegistParam("VC_Bias_Y", 0f);
            parser.RegistParam("VC_SoftZone_Rect", new Rect());
            parser.RegistParam("VC_DeadZone_Rect", new Rect());
            long zoneTimer = sceneTimer.NewFrameTimer(BBTimerInvokeType.CameraDeadZoneTimer, parser);
            token.Add(() =>
            {
                sceneTimer.Remove(ref zoneTimer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}