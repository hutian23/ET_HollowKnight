using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_Confiner_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Confiner";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Confiner: (?<CenterX>.*?), (?<CenterY>.*?), (?<SizeX>.*?), (?<SizeY>.*?);");
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
            
            parser.TryRemoveParam("VC_Confiner_Center");
            parser.TryRemoveParam("VC_Confiner_Size");
            parser.RegistParam("VC_Confiner_Center", new Vector2(centerX, centerY) / 10000f);
            parser.RegistParam("VC_Confiner_Size", new Vector2(sizeX, sizeY) / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}