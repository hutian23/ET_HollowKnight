using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Thrones_WaitFrame_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Thrones_WaitFrame";
        }

        //Thrones_WaitFrame: 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "Thrones_WaitFrame: (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int frame))
            {
                Log.Error($"cannot format {match.Groups["WaitFrame"].Value} to int!!!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            await unit.GetComponent<BBTimerComponent>().WaitAsync(frame, token);
           
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}