﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Thrones_Param_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Thrones_Param";
        }

        //Thrones_Param: 1, MaxV, 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine,@"Thrones_Param: (?<No>.*?), (?<Param>\w+), (?<Value>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            long instanceId = parser.GetParam<long>($"Throne_{match.Groups["No"].Value}");
            TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
            if (timelineComponent.ContainParam("DeadFlag")) return Status.Success;
            
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            buffer.TryRemoveParam(match.Groups["Param"].Value);
            buffer.RegistParam(match.Groups["Param"].Value, match.Groups["Value"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}