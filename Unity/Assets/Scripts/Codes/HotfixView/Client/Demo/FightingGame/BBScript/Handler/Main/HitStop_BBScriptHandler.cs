﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    public class HitStop_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }
        
        //HitStop: 8;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitStop: (?<HitStop>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["HitStop"].Value, out int hitStop))
            {
                Log.Error($"cannot format HitStop to int!");
                return Status.Failed;
            }

            HitStopCor(parser, hitStop, token).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask HitStopCor(BBParser parser,int hitStop, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();

            int hertz = bbTimer.GetHertz();
            bbTimer.SetHertz(0);
            await sceneTimer.WaitAsync(hitStop, token);
            bbTimer.SetHertz(hertz);
        }
    }
}