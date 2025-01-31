using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VC_FollowOffset_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowOffset";
        }

        //VC_
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FollowOffset: (?<OffsetX>.*?), (?<OffsetY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["OffsetX"].Value, out long offsetX) || !long.TryParse(match.Groups["OffsetY"].Value, out long offsetY))
            {
                Log.Error($"cannot format {match.Groups["OffsetX"]} / {match.Groups["OffsetY"].Value} to long!");
                return Status.Failed;
            }

            parser.TryRemoveParam("VC_Follow_Offset");
            parser.RegistParam("VC_Follow_Offset", new Vector2(offsetX / 100f, offsetY / 100f));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}