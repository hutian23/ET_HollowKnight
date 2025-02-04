using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraFollowTimer)]
    [FriendOf(typeof(CameraManager))]
    public class CameraFollowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //1. 更新 Field of View
            float preFOV = self.GetParam<float>("VC_CurrentFOV");
            float minFOV = self.GetParam<float>("VC_MinFOV");
            float maxFOV = self.GetParam<float>("VC_MaxFOV");
            float currentFOV = Mathf.Clamp(preFOV, minFOV, maxFOV);
            Camera.main.orthographicSize = currentFOV;
            self.UpdateParam("VC_CurrentFOV", currentFOV);
            
            // 获取摄像机的视口大小和屏幕宽高
            // 正交距离表示摄像机在世界坐标系中能看到的垂直方向一半的长度
            // 正交距离 = 屏幕高度 / 2
            // 宽度 = 2 * 屏幕宽高比 * 正交距离
            float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
            
            //2. 更新跟随点
            Unit unit = Root.Instance.Get(self.GetParam<long>("VC_Follow_Id")) as Unit;
            // 不存在跟随对象
            if (unit == null || unit.InstanceId == 0) return;
            
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;

            Vector2 targetPosition = go.transform.position;
            float offset = self.GetParam<float>("VC_Follow_CurrentOffset");
            Vector2 center = targetPosition + new Vector2(halfWidth * 2 * offset, 0);
            self.UpdateParam("VC_Follow_TargetPosition", targetPosition);
            self.UpdateParam("VC_Follow_Center", center);
        }
    }
}