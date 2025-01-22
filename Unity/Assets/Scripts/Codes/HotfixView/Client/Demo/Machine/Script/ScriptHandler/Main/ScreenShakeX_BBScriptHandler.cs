using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    [Invoke(BBTimerInvokeType.ScreenShakeXTimer)]
    public class ScreenShakeXTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            BBTimerComponent bbTimer = BBTimerManager.Instance.SceneTimer();
            float shakeLength = CameraManager.instance.shakeLength;
            float frequency = CameraManager.instance.frequency;
            int totalFrame = CameraManager.instance.totalFrame;
            int curFrame = CameraManager.instance.curFrame;
            long timer = CameraManager.instance.timer;
            
            float shakePos = shakeLength * Mathf.Cos(curFrame * frequency / Mathf.PI) * (curFrame / (float)totalFrame);
            CameraManager.instance.MainCamera.transform.position = CameraManager.instance.Position + new Vector3(shakePos, 0, 0);
            
            curFrame--;
            if (curFrame <= 0)
            {
                bbTimer.Remove(ref timer);
                CameraManager.instance.shakeLength = 0;
                CameraManager.instance.frequency = 0;
                CameraManager.instance.totalFrame = 0;
                CameraManager.instance.curFrame = 0;
                CameraManager.instance.timer = 0;
                CameraManager.instance.MainCamera.transform.position = CameraManager.instance.Position;
                return;
            }
            CameraManager.instance.curFrame = curFrame;
        }
    }
    [FriendOf(typeof(CameraManager))]
    public class ScreenShakeX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ScreenShakeX";
        }

        //ScreenShakeX: 1000, 15000, 15; (ShakeLength, Frequency, ShakeFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ScreenShakeX: (?<ShakeLength>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["ShakeLength"].Value, out long shakeLength) ||
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency))
            {
                Log.Error($"cannot format {match.Groups["ShakeFrame"].Value} / {match.Groups["ShakeLength"].Value} / {match.Groups["Frequency"].Value} to long!!");
                return Status.Failed;
            }

            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();

            //1. 初始化
            if (CameraManager.instance.timer != 0)
            {
                sceneTimer.Remove(ref CameraManager.instance.timer);
            }

            //2. 
            long timer = sceneTimer.NewFrameTimer(BBTimerInvokeType.ScreenShakeXTimer, parser);
            CameraManager.instance.timer = timer;
            CameraManager.instance.shakeLength = shakeLength / 10000f;
            CameraManager.instance.frequency = frequency / 10000f;
            CameraManager.instance.totalFrame = shakeFrame;
            CameraManager.instance.curFrame = shakeFrame;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}