namespace ET.Client
{
    public class Self_ParamHandler : BBParamHandler
    {
        public override string GetRefType()
        {
            return "Self";
        }

        public override string Handle(BBParser parser, string param)
        {
            return parser.GetParam<string>(param);
        }
    }
}