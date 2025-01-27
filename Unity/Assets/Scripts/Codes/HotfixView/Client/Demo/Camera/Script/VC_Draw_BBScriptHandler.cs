using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_Draw_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Draw";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Draw: (?<CenterX>.*?), (?<CenterY>.*?), (?<SizeX>.*?), (?<SizeY>.*?);");
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
            
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}