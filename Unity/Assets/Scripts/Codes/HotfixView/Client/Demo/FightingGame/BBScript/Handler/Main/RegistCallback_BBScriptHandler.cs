using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CallbackCheckTimer)]
    [FriendOf(typeof(TimelineCallback))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(TimelineComponent))]
    public class CallbackCheckTimer : BBTimer<TimelineCallback>
    {
        protected override void Run(TimelineCallback self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BBParser parser = timelineComponent.GetComponent<BBParser>();
            BBTimerComponent postStepTimer = b2GameManager.Instance.GetPostStepTimer();

            //exec trigger
            BBScriptData data = BBScriptData.Create(self.trigger, 0, null);
            bool ret = DialogueDispatcherComponent.Instance.GetTrigger(self.triggerType).Check(parser, data);
            data.Recycle();

            if (!ret) return;
            //exec callback
            int startIndex = self.startIndex, endIndex = self.endIndex;
            parser.RegistSubCoroutine(startIndex, endIndex, parser.cancellationToken).Coroutine();

            //Dispose bbCallback
            postStepTimer.Remove(ref self.CheckTimer);
            timelineComponent.callbackDict.Remove(self.callBackName);
            timelineComponent.RemoveChild(self.Id);
        }
    }

    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(TimelineCallback))]
    [FriendOf(typeof(TimelineComponent))]
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
            BBTimerComponent postStepTimer = b2GameManager.Instance.GetPostStepTimer();
            TimelineCallback timelineCallback = timelineComponent.AddChild<TimelineCallback>();
            timelineComponent.callbackDict.Add(callbackName, timelineCallback.Id);

            //3. skip callback group
            int index = bbParser.function_Pointers[data.functionID];
            int endIndex = index, startIndex = index + 1;
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
            timelineCallback.startIndex = startIndex;
            timelineCallback.endIndex = endIndex;
            timelineCallback.callBackName = callbackName;

            //3-1. match trigger
            Match triggerMatch = Regex.Match(trigger, @"(.*?):");
            if (!triggerMatch.Success)
            {
                DialogueHelper.ScripMatchError(trigger);
                return Status.Failed;
            }
            timelineCallback.triggerType = triggerMatch.Groups[1].Value;
            timelineCallback.trigger = trigger;

            //3-2 check callback trigger per frame
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.CallbackCheckTimer, timelineCallback);
            timelineCallback.CheckTimer = timer;
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}