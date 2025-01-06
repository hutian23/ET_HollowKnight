﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    public class NumericSet_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "NumericSet";
        }

        //NumericSet: JumpCount, 2;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"NumericSet: (?<NumericType>.*?), (?<Count>-?\d+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Count"].Value, out long count))
            {
                Log.Error($"cannot format {match.Groups["Count"].Value} to long");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            timelineComponent.UpdateParam(match.Groups["NumericType"].Value, count);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}