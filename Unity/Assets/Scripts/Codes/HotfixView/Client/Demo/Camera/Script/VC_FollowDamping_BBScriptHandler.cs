using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_FollowDamping_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowDamping";
        }

        // VC_FollowDamping: 50000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FollowDamping: (?<DampingX>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["DampingX"].Value, out long dampingX))
            {
                Log.Error($"cannot format {match.Groups["DampingX"].Value} to long!");
                return Status.Failed;
            }
            
            parser.TryRemoveParam("VC_Follow_Damping");
            parser.RegistParam("VC_Follow_Damping", dampingX / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}