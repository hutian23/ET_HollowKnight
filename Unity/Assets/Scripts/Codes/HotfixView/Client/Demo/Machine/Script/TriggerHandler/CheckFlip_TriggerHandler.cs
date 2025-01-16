using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    public class CheckFlip_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Flip";
        }
        
        //Flip: Left
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine,@"Flip: (?<Flip>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            Unit unit = timelineComponent.GetParent<Unit>();
            
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            FlipState checkFlip = match.Groups["Flip"].Value.Equals("Left")? FlipState.Left : FlipState.Right;
            return body.GetFlip() == (int)checkFlip;
        }
    }
}