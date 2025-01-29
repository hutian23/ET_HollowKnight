using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraFollowTimer)]
    [FriendOf(typeof(CameraManager))]
    public class CameraFollowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            Rect deadZoneRect = self.GetParam<Rect>("VC_DeadZone_Rect");
            Camera.main.transform.position = new Vector3(deadZoneRect.center.x, deadZoneRect.center.y, -10);
            
            //1. 设置了相机边界
            if (self.ContainParam("VC_Confiner_Rect"))
            {
                // 获取摄像机的视口大小和屏幕宽高
                // 正交距离表示摄像机在世界坐标系中能看到的垂直方向一半的长度
                // 正交距离 = 屏幕高度 / 2
                // 宽度 = 2 * 屏幕宽高比 * 正交距离
                float HalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
                float HalfHeight = Camera.main.orthographicSize;
                Rect _Rect = new(Camera.main.transform.position - new Vector3(HalfWidth, HalfHeight), new Vector3(HalfWidth, HalfHeight) * 2);
                Rect confinerRect = self.GetParam<Rect>("VC_Confiner_Rect");

                float xMin = Mathf.Clamp(_Rect.xMin, confinerRect.xMin, confinerRect.xMax - _Rect.width);
                float yMin = Mathf.Clamp(_Rect.yMin, confinerRect.yMin, confinerRect.yMax - _Rect.height);
                Rect curRect = new(new Vector2(xMin, yMin), _Rect.size);

                Camera.main.transform.position = new Vector3(curRect.center.x, curRect.center.y, -10);
            }
        }
    }
}