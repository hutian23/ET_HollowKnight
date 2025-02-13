using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RegistCounter_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistCounter";
        }

        // RegistCounter: Cnt_1, 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistCounter: (?<Counter>\w+), (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"cannot format {match.Groups["WaitFrame"].Value} to int!");
                return Status.Failed;
            }

            parser.TryRemoveParam($"Counter_{match.Groups["Counter"].Value}");
            parser.RegistParam($"Counter_{match.Groups["Counter"].Value}", waitFrame);
            
            // 计时器协程
            async ETTask CounterCor()
            {
                BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();
                int counter = waitFrame;
                while (counter-- > 0)
                {
                    parser.UpdateParam($"Counter_{match.Groups["Counter"].Value}", counter);
                    await bbTimer.WaitFrameAsync(token);
                    if (token.IsCancel())
                    {
                        return;
                    }
                }   
            }
            CounterCor().Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}