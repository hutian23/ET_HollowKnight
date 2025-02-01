using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_FollowDelay_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowDelay";
        }

        //VC_FollowDelay: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FollowDelay: (?<Delay>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["Delay"].Value, out int delay))
            {
                Log.Error($"cannot format {match.Groups["Delay"].Value} to int");
                return Status.Failed;
            }

            parser.TryRemoveParam("VC_Follow_Delay");
            parser.RegistParam("VC_Follow_Delay", delay);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}