﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class AddWhiffOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddWhiffOption";
        }

        //AddWhiffOption: 'Mai_Run';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AddWhiffOption: '(?<Option>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetInfoByName(match.Groups["Option"].Value);
            if (info.behaviorOrder == buffer.currentOrder)
            {
                Log.Error($"can not add same behavior into whiffCancel!");
                return Status.Failed;
            }
            
            buffer.WhiffOptions.Add(info.behaviorOrder);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}