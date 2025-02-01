using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraFollowTimer)]
    [FriendOf(typeof(CameraManager))]
    public class CameraFollowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            // 获取摄像机的视口大小和屏幕宽高
            // 正交距离表示摄像机在世界坐标系中能看到的垂直方向一半的长度
            // 正交距离 = 屏幕高度 / 2
            // 宽度 = 2 * 屏幕宽高比 * 正交距离
            float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
            
            //2. 更新跟随点
            Unit unit = Root.Instance.Get(self.GetParam<long>("VC_Follow_Id")) as Unit;
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;

            Vector2 targetPosition = go.transform.position;
            float offset = self.GetParam<float>("VC_Follow_CurrentOffset");
            Vector2 center = targetPosition + new Vector2(halfWidth * 2 * offset, 0);
            self.UpdateParam("VC_Follow_TargetPosition", targetPosition);
            self.UpdateParam("VC_Follow_Center", center);

            //3. 更新朝向
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            if (body.GetFlip() != self.GetParam<int>("VC_Follow_Flip"))
            {
                self.UpdateParam("VC_Follow_Flip", body.GetFlip());
                EventSystem.Instance.Invoke(new UpdateFollowOffsetCallback(){instanceId = self.InstanceId, flip = -body.GetFlip()});
            }
        }
    }
}