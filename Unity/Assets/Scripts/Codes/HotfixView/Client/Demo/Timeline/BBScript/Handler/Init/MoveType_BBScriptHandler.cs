﻿using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    public class MoveType_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MoveType";
        }

        // MoveType: Normal;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"MoveType: (?<MoveType>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!Enum.TryParse(match.Groups["MoveType"].Value, out MoveType moveType))
            {
                Log.Error($"cannot parser {match.Groups["MoveType"].Value} to MoveType");
                return Status.Failed;
            }

            BehaviorBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetChild<BehaviorInfo>(parser.GetParam<long>("InfoId"));
            info.moveType = moveType;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}