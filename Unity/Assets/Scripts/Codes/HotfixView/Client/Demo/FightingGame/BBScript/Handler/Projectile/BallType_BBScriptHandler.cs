using System.Text.RegularExpressions;

namespace ET.Client
{
    public class BallType_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BallType";
        }

        //BallType: SlashRing;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BallType: (?<BallType>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            parser.TryRemoveParam("BallType");
            parser.RegistParam("BallType", match.Groups["BallType"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}