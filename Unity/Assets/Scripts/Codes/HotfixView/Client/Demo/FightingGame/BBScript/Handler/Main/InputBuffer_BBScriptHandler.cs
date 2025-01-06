using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(InputWait))]
    public class InputBuffer_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InputBuffer";
        }

        //InputBuffer: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"InputBuffer: ((?<InputBuffer>\w+));");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            
            if (match.Groups["InputBuffer"].Value.Equals("true"))
            {
                inputWait.BufferFlag = true;
                token.Add(() => { inputWait.BufferFlag = false; });
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}