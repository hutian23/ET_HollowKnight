using System.Numerics;
using System.Text.RegularExpressions;

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

            System.Random random = new();
            float shakeLength = self.GetParam<float>("ShakeX_Length");
            // int totalFrame = self.GetParam<int>("ShakeX_TotalFrame");

            Vector2 shakePos = new Vector2(random.Next(-1, 1), 0) * shakeLength;
            b2Body.UpdateFlag = true;
            b2Body.offset = shakePos;
            Log.Warning(b2Body.offset.ToString());
        }
    }
    [FriendOf(typeof(b2Body))]
    public class ShakeX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ShakeX";
        }

        //Shake_Vertical: 100, 15;(ShakeLength, ShakeFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ShakeX: (?<ShakeLength>.*?), (?<ShakeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["ShakeLength"].Value, out long shakeLength) ||
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame))
            {
                Log.Error($"cannot format {match.Groups["ShakeFrame"].Value} / {match.Groups["ShakeLength"].Value} to long!!");
                return Status.Failed;
            }

            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            b2Body b2Body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);

            //1. 初始化
            parser.TryRemoveParam("ShakeX_Length");
            parser.TryRemoveParam("ShakeX_Frame");
            parser.TryRemoveParam("ShakeX_TotalFrame");
            if (parser.ContainParam("ShakeX_Timer"))
            {
                long _timer = parser.GetParam<long>("ShakeX_Timer");
                postStepTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("ShakeX_Timer");

            //2. 注册变量
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.ShakeXTimer, parser);
            parser.RegistParam("ShakeX_Length", shakeLength / 10000f);
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