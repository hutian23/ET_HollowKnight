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
            
            // DeadZone
            //1. 根据正交距离调整deadZone范围
            Rect deadZone = self.GetParam<Rect>("VC_DeadZone_Rect");
            float halfSizeX = self.GetParam<float>("VC_DeadZone_X") * halfWidth;
            float halfSizeY = self.GetParam<float>("VC_DeadZone_Y") * halfHeight;
            deadZone = new Rect(deadZone.center - new Vector2(halfSizeX, halfSizeY), new Vector2(halfSizeX, halfSizeY) * 2);
            
            // SoftZone
            Rect softZone = self.GetParam<Rect>("VC_SoftZone_Rect");
            float _halfSizeX = self.GetParam<float>("VC_SoftZone_X") * halfWidth;
            float _halfSizeY = self.GetParam<float>("VC_SoftZone_Y") * halfHeight;
            softZone = new Rect(deadZone.center - new Vector2(_halfSizeX, _halfSizeY), new Vector2(_halfSizeX, _halfSizeY) * 2);
            
            self.UpdateParam("VC_DeadZone_Rect", deadZone);
            self.UpdateParam("VC_SoftZone_Rect", softZone);

            // 更新锚点
            if (deadZone.xMin >= center.x && deadZone.xMax <= center.x && deadZone.yMin >= center.y && deadZone.yMax <= center.y) return;
            
            Vector2 prePos = deadZone.position, curPos = prePos;
            //1. X
            {
                float dampingX = self.GetParam<float>("VC_Damping_X");
                // 在softZone区间
                if ((center.x < deadZone.xMin && center.x >= softZone.xMin) ||
                    (center.x > deadZone.xMax && center.x <= softZone.xMax))
                {
                    float targetX = prePos.x;
                    if (center.x <= deadZone.xMin)
                    {
                        targetX = center.x;
                    }
                    else if (center.x >= deadZone.xMax)
                    {
                        targetX = center.x - deadZone.size.x;
                    }

                    curPos.x = Mathf.Lerp(prePos.x, targetX, dampingX * 1 / 60f);
                }
                //超出softZone区间
                else
                {
                    float softX = softZone.xMin;
                    if (center.x <= softZone.xMin)
                    {
                        softX = center.x;
                    }
                    else if (center.x >= softZone.xMax)
                    {
                        softX = center.x - softZone.size.x;
                    }
                    
                    curPos.x = prePos.x + (softX - softZone.xMin);
                }
            }
            //2. Y
            {
                float dampingY = self.GetParam<float>("VC_Damping_Y");
                // 在softZone区间
                if ((center.y < deadZone.yMin && center.y >= softZone.yMin) ||
                    (center.y > deadZone.yMax && center.y <= softZone.yMax))
                {
                    float targetY = prePos.y;
                    if (center.y <= deadZone.yMin)
                    {
                        targetY = center.y;
                    }
                    else if (center.y >= deadZone.yMax)
                    {
                        targetY = center.y - deadZone.size.y;
                    }

                    curPos.y = Mathf.Lerp(prePos.y, targetY, dampingY * 1 / 60f);
                }
                //超出softZone区间
                else
                {
                    float softY = softZone.yMin;
                    if (center.y <= softZone.yMin)
                    {
                        softY = center.y;
                    }
                    else if (center.y >= softZone.yMax)
                    {
                        softY = center.y - softZone.size.y;
                    }
                    
                    curPos.y = prePos.y + (softY - softZone.yMin);
                }
            }
            
            deadZone = new Rect(curPos, new Vector2(halfSizeX, halfSizeY) * 2);
            softZone = new Rect(deadZone.center - new Vector2(_halfSizeX, _halfSizeY), new Vector2(_halfSizeX, _halfSizeY) * 2);
            self.UpdateParam("VC_DeadZone_Rect", deadZone);
            self.UpdateParam("VC_SoftZone_Rect", softZone);
        }
    }
}