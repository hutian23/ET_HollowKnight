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
            long refId = parser.GetParam<long>("BBRef_Id");
            TimelineComponent timelineComponent = Root.Instance.Get(refId) as TimelineComponent;
            return timelineComponent.GetParam<string>(param);
        }
    }
}