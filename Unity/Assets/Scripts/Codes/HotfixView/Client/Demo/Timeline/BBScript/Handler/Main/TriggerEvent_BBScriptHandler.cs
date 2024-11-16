using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(TriggerEvent))]
    [FriendOf(typeof(HitboxComponent))]
    public class TriggerEvent_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TriggerEvent";
        }

        //TriggerEvent: (TriggerStay), (Hurt_Body)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            MatchCollection matches = Regex.Matches(data.opLine, @"\((.*?)\)");
            //语法错误
            if (matches.Count == 0)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            TriggerEvent triggerEvent = hitboxComponent.AddChild<TriggerEvent>();
            hitboxComponent.triggerEventIds.Add(triggerEvent.Id);

            for (int i = 0; i < matches.Count; i++)
            {
                Match match2 = Regex.Match(matches[i].Value, @"\((.*?)\)");
                string param = match2.Groups[1].Value;

                //Trigger Type
                if (i == 0)
                {
                    // TriggerType
                    switch (param)
                    {
                        case "TriggerEnter":
                            triggerEvent.TriggerType = TriggerType.TriggerEnter;
                            break;
                        case "TriggerStay":
                            triggerEvent.TriggerType = TriggerType.TriggerStay;
                            break;
                        case "TriggerExit":
                            triggerEvent.TriggerType = TriggerType.TriggerExit;
                            break;
                    }
                }
                else
                {
                    hitboxComponent.HitboxDict.Add(param, triggerEvent.Id);
                }
            }

            int index = bbParser.function_Pointers[data.functionID];
            for (int i = index; i < bbParser.opDict.Count; i++)
            {
                if (bbParser.opDict[i].Equals("EndTriggerEvent:"))
                {
                    break;
                }
                triggerEvent.opLines.Add(bbParser.opDict[i]);
                bbParser.function_Pointers[data.functionID]++;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}