using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.LoopTimer)]
    [FriendOf(typeof(BBParser))]
    public class LoopTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            int startIndex = self.GetParam<int>("LoopStartIndex");
            string LoopTrigger = self.opDict[startIndex];

            bool result = true;
            
            //1, match trigger
            MatchCollection matches = Regex.Matches(LoopTrigger, @"\((.*?)\)");
            if (matches.Count == 0)
            {
                Log.Error($"Loop_Handler must have at least one triggerHandler!");
                result = false;
            }

            //2. exec trigger handler
            for (int i = 0; i < matches.Count; i++)
            {
                string op = matches[i].Groups[1].Value;
                Match triggerMatch = Regex.Match(op, @"(.*?):");
                //match failed
                if (!triggerMatch.Success)
                {
                    DialogueHelper.ScripMatchError(op);
                    result = false;
                    break; 
                }
                
                BBScriptData _data = BBScriptData.Create(op, 0, null);
                bool ret = DialogueDispatcherComponent.Instance.GetTrigger(triggerMatch.Groups[1].Value).Check(self, _data);
                if (!ret)
                {
                    result = false;
                    break;
                }   
            }

            //3. EndLoop
            if (!result)
            {
                //remove timer
                BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
                long loopTimer = self.GetParam<long>("LoopTimer");
                bbTimer.Remove(ref loopTimer);
                self.RemoveParam("LoopTimer");
                
                //cancel loop coroutine
                ETCancellationToken loopToken = self.GetParam<ETCancellationToken>("LoopToken");
                loopToken.Cancel();
                self.RemoveParam("LoopToken");
                self.RemoveParam("LoopStartIndex");
                self.RemoveParam("LoopEndIndex");
            }
        }
    }

    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(DialogueDispatcherComponent))]
    public class BeginLoop_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BeginLoop";
        }

        //只能在Main携程中使用
        //BeginLoop: (None), (InAir: true), (TransitionCached)
        //EndLoop
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            MatchCollection matches = Regex.Matches(data.opLine, @"\((.*?)\)");
            if (matches.Count == 0)
            {
                Log.Error($"Loop_Handler must have at least one triggerHandler!");
                return Status.Failed;
            }

            //1. 
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            //2. 
            int index = bbParser.function_Pointers[data.functionID];
            int endIndex = index, startIndex = index;
            while (++index < bbParser.opDict.Count)
            {
                string opLine = bbParser.opDict[index];
                if (opLine.Equals("EndLoop:"))
                {
                    endIndex = index;
                    break;
                }
            }

            //3. regist param
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.LoopTimer, bbParser);
            ETCancellationToken loopToken = new();
            bbParser.RegistParam("LoopTimer", timer);
            bbParser.RegistParam("LoopToken", loopToken);
            bbParser.RegistParam("LoopStartIndex", startIndex);
            bbParser.RegistParam("LoopEndIndex", endIndex);

            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
                loopToken.Cancel();
            });

            //4. start loop coroutine
            await LoopCoroutine(bbParser, data);
            //5. pointer go to endIndex
            bbParser.function_Pointers[data.functionID] = endIndex;
            
            return token.IsCancel()? Status.Failed : Status.Success;
        }

        private async ETTask<Status> LoopCoroutine(BBParser self, BBScriptData data)
        {
            while (true)
            {
                Status ret = await LoopStep(self, data);
                if (ret != Status.Success) return Status.Failed;
            }
        }

        private async ETTask<Status> LoopStep(BBParser self, BBScriptData data)
        {
            int startIndex = self.GetParam<int>("LoopStartIndex");
            int endIndex = self.GetParam<int>("LoopEndIndex");
            ETCancellationToken loopToken = self.GetParam<ETCancellationToken>("LoopToken");

            //Loop Cor
            self.function_Pointers[data.functionID] = startIndex;
            while (++self.function_Pointers[data.functionID] < endIndex)
            {
                //Loop 当前帧条件不符合，退出
                if (loopToken.IsCancel()) return Status.Failed;

                //match opType
                int index = self.function_Pointers[data.functionID];
                string opLine = self.opDict[index];
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }
                string opType = match.Value;
                if (opType == "SetMarker") continue;
                
                //match handler
                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler: {opType}");
                    return Status.Failed;
                }
                
                //exec handler
                BBScriptData _data = BBScriptData.Create(opLine, index, null);
                Status ret = await handler.Handle(self, _data, loopToken);
                _data.Recycle();
                
                if (loopToken.IsCancel() || ret == Status.Failed) return Status.Failed;
                if (ret != Status.Success) return ret;
            }
            
            //sleep one frame
            await TimerComponent.Instance.WaitFrameAsync(loopToken);
            return loopToken.IsCancel()? Status.Failed : Status.Success;
        }
    }
}