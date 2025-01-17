﻿using System.Numerics;

namespace ET.Client
{
    [FriendOf(typeof(B2Unit))]
    public static class B2UnitSystem
    {
        public class B2UnitAwakeSystem : AwakeSystem<B2Unit,long>
        {
            protected override void Awake(B2Unit self,long unitId)
            {
                self.unitId = unitId;
                EventSystem.Instance.Invoke(new CreateB2bodyCallback(){instanceId = self.unitId});
            }
        }
        
        public class B2UnitLoadSystem : LoadSystem<B2Unit>
        {
            protected override void Load(B2Unit self)
            {
                self.Init();
                //b2World创建刚体
                EventSystem.Instance.Invoke(new CreateB2bodyCallback(){instanceId = self.unitId});
            }
        }
        
        public class B2UnitDestroySystem : DestroySystem<B2Unit>
        {
            protected override void Destroy(B2Unit self)
            {
                b2WorldManager.Instance.GetBody(self.unitId).SetEnable(false);
            }
        }

        public static void Init(this B2Unit self)
        {
            self.ApplyRootMotion = false;
            self.CollisionBuffer.Clear();
        }

        public class B2UnitPreStepSystem : PreStepSystem<B2Unit>
        {
            protected override void PreStepUpdate(B2Unit self)
            {
                b2Body body = b2WorldManager.Instance.GetBody(self.unitId);
                body.SetVelocity(self.Velocity * new Vector2(- body.GetFlip(), 1) * (self.Hertz / 60f));
            }
        }
        
        public class B2UnitPostStepSystem : PostStepSystem<B2Unit>
        {
            protected override void PosStepUpdate(B2Unit self)
            {
                //清空碰撞信息缓冲区
                self.CollisionBuffer.Clear();
            }
        }
        
        public static Vector2 GetVelocity(this B2Unit self)
        {
            return self.Velocity;
        }

        public static void SetVelocity(this B2Unit self, Vector2 velocity, bool IsRootMotion = false)
        {
            if (self.ApplyRootMotion && !IsRootMotion) return;
            self.Velocity = velocity;
        }
        
        public static void SetVelocityY(this B2Unit self, float velocityY, bool IsRootMotion = false)
        {
            self.SetVelocity(new Vector2(self.Velocity.X, velocityY));
        }

        public static void SetVelocityX(this B2Unit self, float velocityX, bool IsRootMotion = false)
        {
            self.SetVelocity(new Vector2(velocityX, self.Velocity.Y));
        }

        public static int GetHertz(this B2Unit self)
        {
            return self.Hertz;
        }

        public static void SetHertz(this B2Unit self, int hertz)
        {
            self.Hertz = hertz;
        }

        public static void SetApplyRootMotion(this B2Unit self, bool applyRootMotion)
        {
            self.ApplyRootMotion = applyRootMotion;
        }
    }
}