using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_DeadZone_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_DeadZone";
        }

        //VC_DeadZone: 0, 0, 100000, 100000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_DeadZone: (?<CenterX>.*?), (?<CenterY>.*?), (?<SizeX>.*?), (?<SizeY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
                !long.TryParse(match.Groups["SizeX"].Value, out long sizeX) ||
                !long.TryParse(match.Groups["SizeY"].Value, out long sizeY))
            {
                Log.Error($"cannot format to long!");
                return Status.Failed;
            }

            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            
            //1. 初始化
            parser.TryRemoveParam("VC_DeadZone_Rect");
            if (parser.ContainParam("VC_DeadZone_Timer"))
            {
                long _timer = parser.GetParam<long>("VC_DeadZone_Timer");
                sceneTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("VC_DeadZone_Timer");
            
            //2. 注册变量
            parser.RegistParam("VC_DeadZone_Rect", new UnityEngine.Rect(new UnityEngine.Vector2(centerX - sizeX / 2f, centerY - sizeY / 2f) / 10000f, new UnityEngine.Vector2(sizeX,sizeY) / 10000f));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}