﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.MoveXTimer)]
    public class MoveXTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            b2Body b2Body = b2GameManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            float v = self.GetParam<float>("MoveX");

            InputWait bbInput = self.GetParent<TimelineComponent>().GetComponent<InputWait>();
            bool left = bbInput.ContainKey(BBOperaType.LEFT) | bbInput.ContainKey(BBOperaType.UPLEFT) | bbInput.ContainKey(BBOperaType.DOWNLEFT);
            bool right = bbInput.ContainKey(BBOperaType.RIGHT) | bbInput.ContainKey(BBOperaType.UPRIGHT) | bbInput.ContainKey(BBOperaType.DOWNRIGHT);

            if (!left && !right)
            {
                b2Body.SetVelocityX(0);
            }
            else
            {
                b2Body.SetVelocityX(v);   
            }
        }
    }

    //对于 run AirBone这些行为，需要在行为中实时转向并改变速度
    public class MoveX_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MoveX";
        }

        //MoveX: 8.3;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //正则匹配成员变量
            Match match = Regex.Match(data.opLine, @"MoveX:\s*(-?\d+(\.\d+)?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //注册变量
            long.TryParse(match.Groups[1].Value, out long moveX);
            parser.RegistParam("MoveX", moveX / 1000f);

            //启动定时器
            BBTimerComponent bbTimer = parser.GetParent<TimelineComponent>()
                    .GetComponent<InputWait>()
                    .GetComponent<BBTimerComponent>();
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.MoveXTimer, parser);
            token.Add(() => { bbTimer.Remove(ref timer); });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}