using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class ScreenShake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ScreenShake";
        }

        //ScreenShakeX: 1000, 1000, 15000, 15; (ShakeLength_X, ShakeLength_Y, Frequency, ShakeFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ScreenShake: (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["ShakeLength_X"].Value, out long shakeLength_X) ||
                !long.TryParse(match.Groups["ShakeLength_Y"].Value, out long shakeLength_Y) || 
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency))
            {
                Log.Error($"cannot format {match.Groups["ShakeFrame"].Value} / {match.Groups["ShakeLength_X"].Value} / {match.Groups["ShakeLength_Y"].Value} /{match.Groups["Frequency"].Value} to long!!");
                return Status.Failed;
            }

            BBParser _parser = VirtualCamera.Instance.GetParent<Unit>().GetComponent<BBParser>();
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            //1. 初始化
            if (_parser.ContainParam("ScreenShake_Timer"))
            {
                long _timer = _parser.GetParam<long>("ScreenShake_Timer");
                lateUpdateTimer.Remove(ref _timer);
            }
            _parser.TryRemoveParam("ScreenShake_Timer");
            _parser.TryRemoveParam("ScreenShake_LengthX");
            _parser.TryRemoveParam("ScreenShake_LengthY");
            _parser.TryRemoveParam("ScreenShake_Frequency");
            _parser.TryRemoveParam("ScreenShake_TotalFrame");
            _parser.TryRemoveParam("ScreenShake_CurFrame");
            _parser.TryRemoveParam("ScreenShake_ActiveCamera");
            
            //2. 
            long timer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.ScreenShakeTimer, _parser);
            _parser.RegistParam("ScreenShake_Timer", timer);
            _parser.RegistParam("ScreenShake_LengthX", shakeLength_X / 10000f);
            _parser.RegistParam("ScreenShake_LengthY", shakeLength_Y / 10000f);
            _parser.RegistParam("ScreenShake_Frequency", frequency / 10000f);
            _parser.RegistParam("ScreenShake_TotalFrame", shakeFrame);
            _parser.RegistParam("ScreenShake_CurFrame", shakeFrame);
            _parser.RegistParam("ScreenShake_ActiveCamera", Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}