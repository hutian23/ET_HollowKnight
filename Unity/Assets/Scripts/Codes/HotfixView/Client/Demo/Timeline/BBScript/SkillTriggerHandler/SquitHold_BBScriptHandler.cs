namespace ET.Client
{
    public class SquitHold_BBScriptHandler : TriggerHandler
    {
        public override string GetTriggerType()
        {
            return "SquitHold";
        }

        //SquitHold: true;
        public override bool Check(ScriptParser parser, ScriptData data)
        {
            return true;
        }
    }
}