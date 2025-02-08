using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_TargetGroup_MinFOV_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_TargetGroup_MinFOV";
        }

        //CM_TargetGroup_MinFOV: TG_Camera, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_TargetGroup_MinFOV: (?<Camera>\w+), (?<Fov>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Fov"].Value, out long fov))
            {
                Log.Error($"cannot format {match.Groups["Fov"].Value} to long!");
                return Status.Failed;
            }
            
            //1. 查询相机
            Dictionary<string, GameObject> cameraDict = parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryGetValue(match.Groups["Camera"].Value, out GameObject camera))
            {
                Log.Error($"does not exist virtual camera: {match.Groups["Camera"].Value}!!!");
                return Status.Failed;
            }
            
            //2. 
            CinemachineVirtualCamera cm = camera.GetComponent<CinemachineVirtualCamera>();
            cm.GetCinemachineComponent<CinemachineFramingTransposer>().m_MinimumOrthoSize = fov / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}