using System.Text.RegularExpressions;

namespace ET.Client
{
    public class GotoBehavior_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GotoBehavior";
        }

        //GotoBehavior: 'Mai_LandBounce';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"GotoBehavior: '(?<behavior>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            Log.Warning(match.Groups["behavior"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}