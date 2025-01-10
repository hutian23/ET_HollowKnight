using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke]
    public class HandleReplaceParamCallback : AInvokeHandler<ReplaceParamCallback,string>
    {
        public override string Handle(ReplaceParamCallback args)
        {
            BBParser parser = Root.Instance.Get(args.instanceId) as BBParser;

            Match match = Regex.Match(args.content, @"(?<Ref>\w+).(?<Param>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(args.content);
                return string.Empty;
            }
            BBParamHandler handler = ScriptDispatcherComponent.Instance.GetParamHandler(match.Groups["Ref"].Value);
            if (handler == null)
            {
                Log.Error($"not found handler: {match.Groups["Ref"].Value}");
                return string.Empty;
            }
            return handler.Handle(parser, match.Groups["Param"].Value);
        }
    }
}