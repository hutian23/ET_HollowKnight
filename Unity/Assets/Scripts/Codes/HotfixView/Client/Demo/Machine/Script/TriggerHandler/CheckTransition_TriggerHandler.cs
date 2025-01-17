using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckTransition_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Transition";
        }

        //Transition: 'RunToIdle';
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"Transition: '(?<transition>\w+)'");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            string transitionFlag = $"Transition_{match.Groups["transition"].Value}";
            return parser.GetParam<bool>(transitionFlag);

        }
    }
}