using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_Camera_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_Camera";
        }

        // CM_Camera: DefaultCamera, 2DCamera;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_Camera: (?<Name>\w+), (?<Type>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            GameObject _camera = unit.GetComponent<GameObjectComponent>().GameObject;
            
            //1. 生成 虚拟相机
            GameObject _virtualCamera = new(match.Groups["Name"].Value);
            _virtualCamera.transform.SetParent(_camera.transform);
            _virtualCamera.transform.localPosition = new Vector3(0, 0, -10);
            CinemachineVirtualCamera vc = _virtualCamera.AddComponent<CinemachineVirtualCamera>();
            
            //2.
            Transform cameraTarget = parser.GetParam<GameObject>("CM_CameraTarget").transform;
            vc.Follow = cameraTarget;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}