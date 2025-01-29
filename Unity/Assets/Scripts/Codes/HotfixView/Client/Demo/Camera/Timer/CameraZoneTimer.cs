using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraDeadZoneTimer)]
    public class DeadZoneTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            long instanceId = self.GetParam<long>("VC_Follow_Id");
            Unit unit = Root.Instance.Get(instanceId) as Unit;
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            
            float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
            float halfHeight = Camera.main.orthographicSize;
            Vector2 center = go.transform.position;
            
            //1. 根据正交距离调整deadZone范围
            Rect deadZone = self.GetParam<Rect>("VC_DeadZone_Rect");
            float halfSizeX = self.GetParam<float>("VC_DeadZone_X") * halfWidth;
            float halfSizeY = self.GetParam<float>("VC_DeadZone_Y") * halfHeight;
            deadZone = new Rect(deadZone.center - new Vector2(halfSizeX, halfSizeY), new Vector2(halfSizeX, halfSizeY) * 2);

            //2. 目标脱出deadZone，调整deadZone锚点
            if (!deadZone.Contains(center))
            { 
                // Vector2 position = deadZone.position;
                //
                // // x
                // if (center.x < deadZone.xMin)
                // {
                //     position.x = center.x;
                // }
                // else if (center.x > deadZone.xMax)
                // {
                //     position.x = center.x - deadZone.size.x;
                // }
                //
                // // y
                // if (center.y < deadZone.yMin)
                // {
                //     position.y = center.y;
                // }
                // else if (center.y > deadZone.yMax)
                // {
                //     position.y = center.y - deadZone.size.y;
                // }
                //
                deadZone = new Rect(center - new Vector2(halfSizeX, halfSizeY), new Vector2(halfSizeX, halfSizeY) * 2);
            }
            self.UpdateParam("VC_DeadZone_Rect", deadZone);
        }
    }
}