using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ApplyRootMotion_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ApplyRootMotion";
        }

        //ApplyRootMotion: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ApplyRootMotion: (?<Apply>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            B2Unit b2Unit = parser.GetParent<Unit>().GetComponent<B2Unit>();
            switch (match.Groups["Apply"].Value)
            {
                case "true":
                    b2Unit.SetApplyRootMotion(true);
                    break;
                case "false":
                    b2Unit.SetApplyRootMotion(false);
                    break;
                default:
                    Log.Warning("cannot match ApplyRootMotion!!");
                    return Status.Failed;
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}