﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(TimelineComponent))]
    [FriendOf(typeof(TimelineMarkerEvent))]
    public class MarkerEvent_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MarkerEvent";
        }

        //MarkerEvent: (Mai_GroundDash_GC);
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"MarkerEvent: \((.*?)\)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            //跳过动画帧事件的代码块
            int index = parser.function_Pointers[data.functionID];
            int endIndex = index, startIndex = index + 1;
            while (++index < bbParser.opDict.Count)
            {
                string opLine = bbParser.opDict[index];
                if (opLine.Equals("EndMarkerEvent:"))
                {
                    endIndex = index;
                    break;
                }
            }
            bbParser.function_Pointers[data.functionID] = endIndex;

            TimelineMarkerEvent markerEvent = timelineComponent.AddChild<TimelineMarkerEvent>();
            timelineComponent.markerEventDict.Add(match.Groups[1].Value, markerEvent.Id);

            markerEvent.startIndex = startIndex;
            markerEvent.endIndex = endIndex;
            markerEvent.markerName = match.Groups[1].Value;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}