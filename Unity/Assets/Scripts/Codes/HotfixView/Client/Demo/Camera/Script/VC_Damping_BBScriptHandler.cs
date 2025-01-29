using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_Damping_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Damping";
        }

        //VC_Damping: 40000, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Damping: (?<CenterX>.*?), (?<CenterY>.*?);");
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
            parser.TryRemoveParam("VC_Damping_X");
            parser.TryRemoveParam("VC_Damping_Y");
            //2. 注册变量
            parser.RegistParam("VC_Damping_X", centerX / 100f);
            parser.RegistParam("VC_Damping_Y", centerY / 100f);
           
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}