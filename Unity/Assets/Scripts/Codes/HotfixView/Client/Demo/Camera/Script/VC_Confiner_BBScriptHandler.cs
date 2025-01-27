using System.Text.RegularExpressions;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    public class VC_Confiner_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Confiner";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Confiner: (?<CenterX>.*?), (?<CenterY>.*?), (?<SizeX>.*?), (?<SizeY>.*?);");
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
                Log.Error($"cannot format to long!");
                return Status.Failed;
            }

            parser.TryRemoveParam("VC_Confiner_Rect");
            parser.RegistParam("VC_Confiner_Rect", 
                new Rect(new UnityEngine.Vector2(centerX - sizeX / 2f, centerY - sizeY / 2f) / 10000f,
                            new UnityEngine.Vector2(sizeX,sizeY) / 10000f));
            
            //1. Gizmos
            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            PolygonShape shape = new();
            shape.SetAsBox(sizeX / 20000f, sizeY / 20000f, new Vector2(centerX, centerY) / 10000f, 0f);
            FixtureDef def = new() 
            {
                Shape = shape, 
                Density = 0.0f, 
                Friction = 0.0f, 
                UserData = new FixtureData()
                {
                    InstanceId = body.InstanceId,
                    Name = "VN_Confiner",
                    UserData = new BoxInfo()
                    {
                        boxName = "VN_Confiner",
                        center = new UnityEngine.Vector2(centerX, centerY) / 10000f,
                        size = new UnityEngine.Vector2(sizeX, sizeY) / 10000f,
                        hitboxType = HitboxType.Gizmos
                    }
                }
            };
            body.CreateFixture(def);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}