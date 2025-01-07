namespace ET.Client
{
    [Invoke]
    public class HandleReplaceParamCallback : AInvokeHandler<ReplaceParamCallback,string>
    {
        public override string Handle(ReplaceParamCallback args)
        {
            BBParser parser = Root.Instance.Get(args.instanceId) as BBParser;

            BBParamHandler handler = ScriptDispatcherComponent.Instance.GetParamHandler(args.refName);
            if (handler == null)
            {
                Log.Error($"not found handler: {args.refName}");
                return string.Empty;
            }

            return handler.Handle(parser, args.content);
        }
    }
}