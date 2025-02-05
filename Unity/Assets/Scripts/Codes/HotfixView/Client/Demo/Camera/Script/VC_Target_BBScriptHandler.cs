using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VC_Target_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Target";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Target: (?<posX>.*?), (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["posX"].Value, out long posX) || !long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"cannot format {match.Groups["posX"].Value} / {match.Groups["posY"].Value} to long!");
                return Status.Failed;
            }
            
            BBParser _parser = VirtualCamera.Instance.GetParent<Unit>().GetComponent<BBParser>();
            ListComponent<Vector2> points = _parser.GetParam<ListComponent<Vector2>>("VC_Points");
            points.Add(new Vector2(posX, posY) / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}