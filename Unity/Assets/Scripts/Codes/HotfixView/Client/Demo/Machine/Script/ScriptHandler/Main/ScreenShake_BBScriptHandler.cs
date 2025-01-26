using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class ScreenShake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ScreenShakeX";
        }

        //ScreenShakeX: 1000, 1000, 15000, 15; (ShakeLength_X, ShakeLength_Y, Frequency, ShakeFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ScreenShakeX: (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?);");
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

            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            //1. 初始化
            if (CameraManager.instance.timer != 0)
            {
                sceneTimer.Remove(ref CameraManager.instance.timer);
            }
            //2. 
            long timer = sceneTimer.NewFrameTimer(BBTimerInvokeType.ScreenShakeTimer, parser);
            CameraManager.instance.timer = timer;
            CameraManager.instance.shakeLength_X = shakeLength_X / 10000f;
            CameraManager.instance.shakeLength_Y = shakeLength_Y / 10000f;
            CameraManager.instance.frequency = frequency / 10000f;
            CameraManager.instance.totalFrame = shakeFrame;
            CameraManager.instance.curFrame = shakeFrame;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}