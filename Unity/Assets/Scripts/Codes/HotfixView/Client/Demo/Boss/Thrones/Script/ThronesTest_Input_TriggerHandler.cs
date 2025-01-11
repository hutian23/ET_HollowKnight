using System.Text.RegularExpressions;
using UnityEngine.InputSystem;

namespace ET.Client
{
    public class ThronesTest_Input_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "ThronesTest_Input";
        }

        //ThronesInput: A
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"ThronesTest_Input: (?<InputType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            switch (match.Groups["InputType"].Value)
            {
                case "A":
                    return Keyboard.current.aKey.isPressed;
                case "S":
                    return Keyboard.current.sKey.isPressed;
                case "D":
                    return Keyboard.current.dKey.isPressed;
                default:
                    return false;
            }
        }
    }
}