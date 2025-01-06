using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class ScreenShake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ScreenShake";
        }

        //ScreenShake: ShakeLength, shakeCnt;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ScreenShake: (?<ShakeLength>\w+), (?<ShakeCnt>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["ShakeLength"].Value, out int shakeLength) ||
                !int.TryParse(match.Groups["ShakeCnt"].Value, out int shakeCnt))
            {
                Log.Error($"cannot format {match.Groups["ShakeLength"].Value} / {match.Groups["ShakeCnt"].Value} to int!");
                return Status.Failed;
            }
            CameraManager.instance.shakeLength = shakeLength;
            CameraManager.instance.shakeCnt = shakeCnt;
            CameraManager.instance.shakeTotalCnt = shakeCnt;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}