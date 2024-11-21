using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (InputWait))]
    public class RegistInput_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistInput";
        }

        //RegistInput: '236P',5;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistInput: (?<InputType>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            
            //启动输入检测携程
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            BBInputHandler handler = DialogueDispatcherComponent.Instance.GetInputHandler(match.Groups["InputType"].Value);
            inputWait.InputCheckCor(handler,inputWait.Token).Coroutine();    
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}