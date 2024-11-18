using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CallbackCheckTimer)]
    [FriendOf(typeof(BBCallback))]
    [FriendOf(typeof(BBParser))]
    public class CallbackCheckTimer : BBTimer<BBCallback>
    {
        protected override void Run(BBCallback self)
        {
            BBParser parser = self.GetParent<BBParser>();
            BBTimerComponent bbTimer = parser.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            
            //exec trigger
            BBScriptData data = BBScriptData.Create(self.trigger, 0, null);
            bool ret = DialogueDispatcherComponent.Instance.GetTrigger(self.triggerType).Check(parser, data);
            data.Recycle();

            if (!ret) return;
            //exec callback
            int startIndex = self.startIndex, endIndex = self.endIndex;
            parser.RegistSubCoroutine(startIndex, endIndex, "CallbackCoroutine").Coroutine();
            
            //Dispose bbCallback
            bbTimer.Remove(ref self.CheckTimer);
            parser.callBackDict.Remove(self.callBackName);
            parser.RemoveChild(self.Id);
        }
    }

    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BBCallback))]
    public class RegistCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistCallback";
        }

        //RegistCallback: (InAir: true), 'AirCheckCallback';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            MatchCollection matches = Regex.Matches(data.opLine, @"\((.*?)\)|'([^']*)'");
            if (matches.Count == 0)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. match trigger and callbackName
            string trigger = string.Empty, callbackName = string.Empty;
            foreach (Match match in matches)
            {
                if (match.Groups[1].Success)
                {
                    trigger = match.Groups[1].Value;
                }
                if (match.Groups[2].Success)
                {
                    callbackName = match.Groups[2].Value;
                }
            }

            if (string.IsNullOrEmpty(trigger) || string.IsNullOrEmpty(callbackName))
            {
                Log.Error($"cannot match trigger or callbackName");
                return Status.Failed;
            }

            //2. regist callback entity
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBCallback bbCallback = bbParser.AddChild<BBCallback>();
            bbParser.callBackDict.Add(callbackName, bbCallback);

            //3. skip callback group
            int index = bbParser.function_Pointers[data.functionID];
            int endIndex = index, startIndex = index+1;
            while (++index < bbParser.opDict.Count)
            {
                string opLine = bbParser.opDict[index];
                if (opLine.Equals("EndCallback:"))
                {
                    endIndex = index;
                    break;
                }
            }
            bbParser.function_Pointers[data.functionID] = endIndex;
            
            //3. init bbCallback
            bbCallback.startIndex = startIndex;
            bbCallback.endIndex = endIndex;
            bbCallback.callBackName = callbackName;
            
            //3-1. match trigger
            Match triggerMatch = Regex.Match(trigger, @"(.*?):");
            if (!triggerMatch.Success)
            {
                DialogueHelper.ScripMatchError(trigger);
                return Status.Failed;
            }
            bbCallback.triggerType = triggerMatch.Groups[1].Value;
            bbCallback.trigger = trigger;
            
            //3-2 check callback trigger per frame
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.CallbackCheckTimer, bbCallback);
            bbCallback.CheckTimer = timer;
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}