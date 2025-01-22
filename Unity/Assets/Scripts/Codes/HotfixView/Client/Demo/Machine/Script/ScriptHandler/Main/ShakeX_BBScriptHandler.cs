using System.Text.RegularExpressions;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ShakeXTimer)]
    [FriendOf(typeof(b2Body))]
    public class ShakeXTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            Unit unit = self.GetParent<Unit>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            
            float shakeLength = self.GetParam<float>("ShakeX_Length");
            float frequency = self.GetParam<float>("ShakeX_Frequency");
            int curFrame = self.GetParam<int>("ShakeX_Frame");
            int totalFrame = self.GetParam<int>("ShakeX_TotalFrame");
            long timer = self.GetParam<long>("ShakeX_Timer");
            
            
            //1. 振幅
            Vector2 shakePos = shakeLength * new Vector2(Mathf.Cos(curFrame * frequency) * (curFrame / (float)totalFrame), 0);
            b2Body.UpdateFlag = true;
            b2Body.offset = shakePos;
            
            //2. 振动效果执行完毕?
            curFrame--;
            //销毁相关变量
            if (curFrame <= 0)
            {
                postStepTimer.Remove(ref timer);
                self.TryRemoveParam("ShakeX_Timer");
                self.TryRemoveParam("ShakeX_Frame");
                self.TryRemoveParam("ShakeX_TotalFrame");
                self.TryRemoveParam("ShakeX_Frequency");
                self.TryRemoveParam("ShakeX_Length");
                return;
            }
            
            //3. 更新计时器
            self.UpdateParam("ShakeX_Frame", curFrame);
        }
    }
    [FriendOf(typeof(b2Body))]
    public class ShakeX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ShakeX";
        }

        //ShakeX: 1000, 15000, 15;(ShakeLength, Frequency, ShakeFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ShakeX: (?<ShakeLength>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["ShakeLength"].Value, out long shakeLength) ||
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency))
            {
                Log.Error($"cannot format {match.Groups["ShakeFrame"].Value} / {match.Groups["ShakeLength"].Value} / {match.Groups["Frequency"].Value} to long!!");
                return Status.Failed;
            }

            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            b2Body b2Body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);

            //1. 初始化
            parser.TryRemoveParam("ShakeX_Length");
            parser.TryRemoveParam("ShakeX_Frame");
            parser.TryRemoveParam("ShakeX_TotalFrame");
            parser.TryRemoveParam("ShakeX_Frequency");
            if (parser.ContainParam("ShakeX_Timer"))
            {
                long _timer = parser.GetParam<long>("ShakeX_Timer");
                postStepTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("ShakeX_Timer");

            //2. 注册变量
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.ShakeXTimer, parser);
            parser.RegistParam("ShakeX_Length", shakeLength / 10000f);
            parser.RegistParam("ShakeX_Frequency", frequency / 10000f);
            parser.RegistParam("ShakeX_Frame", shakeFrame);
            parser.RegistParam("ShakeX_TotalFrame", shakeFrame);
            parser.RegistParam("ShakeX_Timer", timer);

            //3. 切换行为时销毁振动效果
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
                b2Body.UpdateFlag = true;
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}