using System.Text.RegularExpressions;
using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;
using Random = System.Random;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ShakeTimer)]
    [FriendOf(typeof(b2Body))]
    public class ShakeTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();

            //销毁shake effect
            long shakeCnt = self.GetParam<long>("ShakeCnt");
            self.UpdateParam("ShakeCnt", --shakeCnt);

            if (shakeCnt <= 0f)
            {
                long shakeTimer = self.GetParam<long>("ShakeTimer");
                sceneTimer.Remove(ref shakeTimer);
                self.TryRemoveParam("ShakeTimer");
                self.TryRemoveParam("ShakeCnt");
                self.TryRemoveParam("Shake");
                self.TryRemoveParam("ShakeLength");
                return;
            }

            int shakeLength = self.GetParam<int>("ShakeLength");
            Random random = new();
            Vector3 shakePos = new(random.Next(-shakeLength, shakeLength) / 25000f, random.Next(-shakeLength, shakeLength) / 25000f);

            b2Body b2Body = b2GameManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            GameObject go = self.GetParent<TimelineComponent>().GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            go.transform.position = b2Body.trans.Position.ToUnityVector3() + shakePos;
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

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            long timer = sceneTimer.NewFrameTimer(BBTimerInvokeType.ShakeTimer, bbParser);
            bbParser.RegistParam("Shake", Vector2.zero);
            bbParser.RegistParam("ShakeCnt", shakeTimer);
            bbParser.RegistParam("ShakeTimer", timer);
            bbParser.RegistParam("ShakeLength", ShakeLength);
            
            token.Add(() =>
            {
                sceneTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}