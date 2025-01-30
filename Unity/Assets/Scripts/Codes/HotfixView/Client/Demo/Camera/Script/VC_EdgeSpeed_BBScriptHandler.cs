using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_EdgeSpeed_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_EdgeSpeed";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_EdgeSpeed: (?<CenterX>.*?), (?<CenterY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY))
            {
                Log.Error($"cannot format to long!");
                return Status.Failed;
            }
            //1. 初始化
            parser.TryRemoveParam("VC_EdgeSpeed_X");
            parser.TryRemoveParam("VC_EdgeSpeed_Y");
            //2. 注册变量
            parser.RegistParam("VC_EdgeSpeed_X", centerX / 10000f);
            parser.RegistParam("VC_EdgeSpeed_Y", centerY / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}