using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_2DCamera_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_2DCamera";
        }

        // CM_Camera: DefaultCamera, 2DCamera;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_2DCamera: (?<Name>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            BBTimerComponent gizmosTimer = b2WorldManager.Instance.GetGizmosTimer();
            Unit unit = parser.GetParent<Unit>();
            GameObject _camera = unit.GetComponent<GameObjectComponent>().GameObject;
            
            //1. 生成 虚拟相机
            GameObject _virtualCamera = new(match.Groups["Name"].Value);
            _virtualCamera.transform.SetParent(_camera.transform);
            CinemachineVirtualCamera vc = _virtualCamera.AddComponent<CinemachineVirtualCamera>();

            Dictionary<string, GameObject> cameraDict = parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryAdd(match.Groups["Name"].Value, _virtualCamera))
            {
                Log.Error($"already exist virtualCamera: {match.Groups["Name"].Value}");
                return Status.Failed;
            }
            
            //2. 初始化
            vc.AddCinemachineComponent<CinemachineTransposer>().enabled = true;
            vc.AddCinemachineComponent<CinemachineComposer>().enabled = true;
            // 设置跟随对象
            Transform cameraTarget = parser.GetParam<GameObject>("CM_CameraTarget").transform;
            vc.Follow = cameraTarget;
            vc.LookAt = cameraTarget;
            
            
            //?. 编辑器内
            long timer = gizmosTimer.NewFrameTimer(BBTimerInvokeType.CameraGizmosTimer, parser);
            token.Add(() =>
            {
                gizmosTimer.Remove(ref timer);
            });
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}