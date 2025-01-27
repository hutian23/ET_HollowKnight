using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class VC_Zoom_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Zoom";
        }

        //VC_Zoom: 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Zoom: (?<Size>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Size"].Value, out long size))
            {
                Log.Error($"cannot format {match.Groups["Size"].Value} to long!");
                return Status.Failed;
            }

            CameraManager.instance.MainCamera.orthographicSize = size / 10000f;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}