namespace ET.Client
{
    public class Machine_ParamHandler : BBParamHandler
    {
        public override string GetRefType()
        {
            return "Machine";
        }

        public override string Handle(BBParser parser, string param)
        {
            return parser.GetParent<Unit>().GetComponent<BehaviorMachine>().GetParam<string>(param);
        }
    }
}