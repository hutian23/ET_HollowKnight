using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HitPushBackTimer)]
    public class PushBackTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            B2Unit b2Unit = timelineComponent.GetComponent<B2Unit>();
            
            float startV = self.GetParam<float>("PushBack_V");
            float friction = self.GetParam<float>("PushBack_F");
            
            float absV = Math.Abs(startV);
            float dv = friction * (1 / 60f);
            float curV = absV < dv ? 0 : (absV - dv) * Math.Sign(startV);
            b2Unit.SetVelocityX(curV);
            self.UpdateParam("PushBack_V", curV);
        }
    }
    [FriendOf(typeof(b2Body))]
    public class PushBack_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PushBack";
        }

        //HitPushBack: 2000, 8;(StartVelocity, Friction)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"PushBack: (?<StartVelocity>-?\d+), (?<Friction>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["StartVelocity"].Value, out long vel) || !long.TryParse(match.Groups["Friction"].Value,out long friction))
            {
                Log.Error($"cannot format {match.Groups["StartVelocity"].Value} / {match.Groups["Friction"].Value} to long!!");
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            parser.TryRemoveParam("PushBack_V");
            parser.TryRemoveParam("PushBack_F");
            parser.RegistParam("PushBack_V", vel / 1000f);
            parser.RegistParam("PushBack_F", friction / 1000f);
            
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.HitPushBackTimer, parser);
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}