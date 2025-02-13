using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckCounter_BBScriptHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Counter";
        }

        
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Counter: (?<Cnt>\w+) (?<Sign>[><=]+) (?<CheckVel>-?\d+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            if (!int.TryParse(match.Groups["CheckVel"].Value, out int checkVel))
            {
                Log.Error($"cannot format {match.Groups["Position"].Value} to int !!!");
                return false;
            }
            
            if (!parser.ContainParam($"Counter_{match.Groups["Cnt"].Value}"))
            {
                return false;
            }

            int targetVel = parser.GetParam<int>($"Counter_{match.Groups["Cnt"].Value}");
            switch (match.Groups["Sign"].Value)
            {
                case "=":
                    return targetVel == checkVel;
                case ">":
                    return targetVel > checkVel;
                case "<":
                    return targetVel < checkVel;
                case ">=":
                    return targetVel >= checkVel;
                case "<=":
                    return targetVel <= checkVel;
                default:
                    return false;
            }
        }
    }
}