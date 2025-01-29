using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_Bias_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Bias";
        }

        //VC_Bias: 30, 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Bias: (?<CenterX>.*?), (?<CenterY>.*?);");
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
            parser.TryRemoveParam("VC_Bias_X");
            parser.TryRemoveParam("VC_Bias_Y");
            //2. 注册变量
            parser.RegistParam("VC_Bias_X", centerX / 100f);
            parser.RegistParam("VC_Bias_Y", centerY / 100f);
           
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}