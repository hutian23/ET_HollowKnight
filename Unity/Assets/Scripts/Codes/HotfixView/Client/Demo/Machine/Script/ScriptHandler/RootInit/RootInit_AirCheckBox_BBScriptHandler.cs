using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AirCheckTimer)]
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(BBParser))]
    public class AirCheckTimer : BBTimer<TimelineComponent>
    {
        protected override void Run(TimelineComponent self)
        {
            B2Unit b2Unit = self.GetComponent<B2Unit>();
            BBParser parser = self.GetComponent<BBParser>();

            Queue<CollisionInfo> infoQueue = b2Unit.CollisionBuffer;
            int count = infoQueue.Count;
            while (count-- > 0)
            {
                bool enableAirCheck = self.GetParam<bool>("EnableAirCheck");
                if (!enableAirCheck) break;
                
                CollisionInfo info = infoQueue.Dequeue();
                infoQueue.Enqueue(info);

                if (info.dataA.Name.Equals("AirCheckBox") && info.dataB.LayerMask is LayerType.Ground && !info.dataB.IsTrigger)
                {
                    //落地回调
                    if (self.GetParam<bool>("InAir"))
                    {
                        self.UpdateParam("InAir", false);
                        if (self.ContainParam("LandCallback_StartIndex"))
                        {
                            int startIndex = self.GetParam<int>("LandCallback_StartIndex");
                            int endIndex = self.GetParam<int>("LandCallback_EndIndex");
                            parser.RegistSubCoroutine(startIndex, endIndex, parser.CancellationToken).Coroutine();
                        }
                    }
                    return;
                }
            }

            self.UpdateParam("InAir", true);
        }
    }

    [FriendOf(typeof(TimelineComponent))]
    public class RootInit_AirCheckBox_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirCheckBox";
        }

        // GroundCheckBox: 0, -2200, 2000, 1000; (Center), (Size)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AirCheckBox: (?<CenterX>-?\d+), (?<CenterY>-?\d+), (?<SizeX>-?\d+), (?<SizeY>-?\d+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
                !long.TryParse(match.Groups["SizeX"].Value, out long sizeX) ||
                !long.TryParse(match.Groups["SizeY"].Value, out long sizeY))
            {
                Log.Error($"cannot format {match.Groups["SizeX"]} / {match.Groups["SizeY"]} / {match.Groups["CenterX"]} / {match.Groups["CenterY"]} to long!! ");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            //1. 注册变量
            timelineComponent.RegistParam("InAir", false);
            timelineComponent.RegistParam("EnableAirCheck", true);
            
            //2. 创建夹具
            PolygonShape shape = new();
            shape.SetAsBox(sizeX / 2000f, sizeY / 2000f, new Vector2(centerX, centerY) / 1000f, 0f);
            FixtureDef fixtureDef = new()
            {
                Shape = shape,
                Density = 1.0f,
                Friction = 0.0f,
                UserData = new FixtureData()
                {
                    InstanceId = body.InstanceId,
                    Name = "AirCheckBox",
                    Type = FixtureType.Default,
                    LayerMask = LayerType.Unit,
                    IsTrigger = true,
                    UserData = new BoxInfo()
                    {
                        boxName = "AirCheckBox",
                        center = new UnityEngine.Vector2(centerX, centerY) / 1000f,
                        size = new UnityEngine.Vector2(sizeX, sizeY) / 1000f,
                        hitboxType = HitboxType.Other
                    },
                    TriggerStayId = TriggerStayType.CollisionEvent
                }
            };
            Fixture fixture = body.CreateFixture(fixtureDef);
            timelineComponent.RegistParam("AirCheckBox", fixture);
            
            //3. 创建定时器
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.AirCheckTimer, timelineComponent);
            timelineComponent.Token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}