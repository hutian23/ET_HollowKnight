﻿using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RootInit_SetPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetPos";
        }

        //InitPos: 3100, 1200;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SetPos: (?<posX>.*?), (?<posY>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["posX"].Value, out long posX) || !long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"cannot format posX / posY to long");
                return Status.Failed;
            }
            Vector2 pos = new Vector2(posX, posY) / 1000f;

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            b2Body.SetPosition(pos);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}