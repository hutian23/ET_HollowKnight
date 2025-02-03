using System;
using UnityEngine;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraOffsetTransitionTimer)]
    public class FollowTransitionTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            Unit unit = Root.Instance.Get(self.GetParam<long>("VC_Follow_Id")) as Unit;
            b2Body _body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            //1. 当前速度小于阈值，不会进行transition
            float vel = Mathf.Abs(_body.GetVelocity().X);
            float minVel = self.GetParam<float>("VC_FollowTransition_MinVel");
            if (vel < minVel) return;
            
            int flip = Math.Sign(_body.GetVelocity().X);
            float offset = self.GetParam<float>("VC_Follow_Offset");
            float accel = self.GetParam<float>("VC_FollowTransition_Accel");
            // float transVel = self.GetParam<float>("VC_FollowTransition_TransVel");
            float preOffset = self.GetParam<float>("VC_Follow_CurrentOffset");
            float maxVel = self.GetParam<float>("VC_FollowTransition_MaxVel");
            
            //2. 速度方向突变，transVel置为0
            // if (Math.Sign(transVel) != flip) transVel = 0f;
            
            //3. 根据朝向调整 FollowOffset
            // float currentOffset = Mathf.Clamp(preOffset + transVel / 60f, -offset, offset);
            float currentOffset = Mathf.Clamp(preOffset + accel / 60f * flip, -offset, offset);
            self.UpdateParam("VC_Follow_CurrentOffset", currentOffset);
            // self.UpdateParam("VC_FollowTransition_TransVel", Math.Clamp(transVel + accel * flip, -maxVel, maxVel));
        }
    }
}