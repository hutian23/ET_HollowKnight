using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ShakeTimer)]
    public class ShakeTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            b2Body b2Body =  b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            //销毁shake effect
            long shakeCnt = timelineComponent.GetParam<long>("ShakeCnt");
            timelineComponent.UpdateParam("ShakeCnt",--shakeCnt);
            
            if (shakeCnt <= 0f)
            {
                long shakeTimer = timelineComponent.GetParam<long>("ShakeTimer");
                bbTimer.Remove(ref shakeTimer);
                timelineComponent.TryRemoveParam("ShakeTimer");
                timelineComponent.TryRemoveParam("ShakeCnt");
                timelineComponent.TryRemoveParam("Shake");
                timelineComponent.TryRemoveParam("ShakeLength");
                b2Body.SetUpdateFlag();
                return;
            }
            
            int shakeLength = timelineComponent.GetParam<int>("ShakeLength");
            Random random = new();
            Vector2 shakePos = new(random.Next(-shakeLength,shakeLength ) / 25000f, random.Next(-shakeLength, shakeLength) / 25000f);
            timelineComponent.UpdateParam("Shake",shakePos);
            b2Body.SetUpdateFlag();
        }
    }

    public class Shake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Shake";
        }

        //Shake: ShakeLength, totalTime;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Shake: (?<ShakeLength>\w+), (?<ShakeCnt>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["ShakeCnt"].Value, out long shakeTimer))
            {
                Log.Error($"cannot format {match.Groups["ShakeCnt"].Value} to long");
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["ShakeLength"].Value, out int ShakeLength))
            {
                Log.Error($"cannot format {match.Groups["ShakeLength"].Value} to long");
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.ShakeTimer, bbParser);
            timelineComponent.RegistParam("Shake", Vector2.zero);
            timelineComponent.RegistParam("ShakeCnt", shakeTimer);
            timelineComponent.RegistParam("ShakeTimer", timer);
            timelineComponent.RegistParam("ShakeLength", ShakeLength);
            
            token.Add(() =>
            {
                timelineComponent.TryRemoveParam("Shake");
                timelineComponent.TryRemoveParam("ShakeCnt");
                timelineComponent.TryRemoveParam("ShakeTimer");
                timelineComponent.TryRemoveParam("ShakeLength");
                b2Body b2Body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
                b2Body.SetUpdateFlag();
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}