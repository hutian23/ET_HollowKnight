using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraTargetTimer)]
    [FriendOf(typeof(CameraManager))]
    public class CameraTargetTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            b2Body b2Body = b2WorldManager.Instance.GetBody(self.InstanceId);
            CameraManager.instance.Position = new Vector3(b2Body.GetPosition().X, b2Body.GetPosition().Y, -10);
            CameraManager.instance.MainCamera.transform.position = CameraManager.instance.Position;
        }
    }

    [FriendOf(typeof(CameraManager))]
    public class CameraTarget_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CameraTarget";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            CameraManager.instance.targetTimer = sceneTimer.NewFrameTimer(BBTimerInvokeType.CameraTargetTimer, parser.GetParent<Unit>());
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}