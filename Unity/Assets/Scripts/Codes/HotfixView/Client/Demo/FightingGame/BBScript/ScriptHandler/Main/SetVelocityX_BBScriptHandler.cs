using System.Text.RegularExpressions;

namespace ET.Client
{
    public class SetVelocityX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetVelocityX";
        }

        //SetVelocityX: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetVelocityX: (?<Velocity>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
        
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
            
            if (!long.TryParse(match.Groups["Velocity"].Value, out long velX))
            {
                Log.Error($"cannot format {match.Groups["Velocity"].Value} to long !!!");
                return Status.Failed;
            }
            b2Unit.SetVelocityX(velX / 1000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}