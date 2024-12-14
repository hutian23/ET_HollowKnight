using System.Text.RegularExpressions;
using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;
using Random = System.Random;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ShakeTimer)]
    public class ShakeTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //对应组件
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            Unit unit = timelineComponent.GetParent<Unit>();
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            //缓存变量
            int shakeCnt = self.GetParam<int>("ShakeCnt");
            int totalShakeCnt = self.GetParam<int>("TotalShakeCnt");
            int ShakeLength = self.GetParam<int>("ShakeLength");
            Vector3 Pos = body.GetPosition().ToUnityVector3();
            Random random = new();
            
            //Dispose
            if (shakeCnt < 0)
            {
                long timer = self.GetParam<long>("ShakeTimer");
                postStepTimer.Remove(ref timer);
                self.TryRemoveParam("ShakeTimer");
                self.TryRemoveParam("ShakeCnt");
                self.TryRemoveParam("TotalShakeCnt");
                self.TryRemoveParam("ShakeLength");
                return;
            }
            
            Vector3 shakePos = new Vector3(
                random.Next(-ShakeLength, ShakeLength),
                random.Next(-ShakeLength, ShakeLength),
                0) / 1000f;
            go.transform.position = Pos + shakePos * shakeCnt / totalShakeCnt;
      
            self.UpdateParam("ShakeCnt", --shakeCnt);
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
            //初始化
            BBTimerComponent PostStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            if (parser.ContainParam("ShakeTimer"))
            {
                long preTimer = parser.GetParam<long>("ShakeTimer");
                PostStepTimer.Remove(ref preTimer);
            }
            parser.TryRemoveParam("ShakeTimer");
            parser.TryRemoveParam("ShakeCnt");
            parser.TryRemoveParam("TotalShakeCnt");
            parser.TryRemoveParam("ShakeLength");
            
            Match match = Regex.Match(data.opLine, @"Shake: (?<ShakeLength>\w+), (?<ShakeCnt>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["ShakeCnt"].Value, out int shakeCnt))
            {
                Log.Error($"cannot format {match.Groups["ShakeCnt"].Value} to long");
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["ShakeLength"].Value, out int ShakeLength))
            {
                Log.Error($"cannot format {match.Groups["ShakeLength"].Value} to long");
                return Status.Failed;
            }

            long timer = PostStepTimer.NewFrameTimer(BBTimerInvokeType.ShakeTimer, parser);
            parser.RegistParam("ShakeTimer", timer);
            parser.RegistParam("ShakeCnt", shakeCnt);
            parser.RegistParam("TotalShakeCnt", shakeCnt);
            parser.RegistParam("ShakeLength", ShakeLength);
            
            token.Add(() =>
            {
                PostStepTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}