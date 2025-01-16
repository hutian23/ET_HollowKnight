namespace ET.Client
{
    public class Timeline_ParamHandler : BBParamHandler
    {
        public override string GetRefType()
        {
            return "Timeline";
        }

        public override string Handle(BBParser parser, string param)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            return timelineComponent.GetParam<string>(param);
        }
    }
}