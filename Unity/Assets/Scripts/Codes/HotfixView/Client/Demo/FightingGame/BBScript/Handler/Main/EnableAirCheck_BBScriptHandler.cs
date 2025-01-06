using System.Text.RegularExpressions;

namespace ET.Client
{
    public class EnableAirCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableAirCheck";
        }

        //EnableAirCheck: false;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableAirCheck: (?<AirCheck>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            switch (match.Groups["AirCheck"].Value)
            {
                case "true":
                    timelineComponent.UpdateParam("EnableAirCheck", true);
                    break;
                case "false":
                    timelineComponent.UpdateParam("EnableAirCheck", false);
                    break;
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}