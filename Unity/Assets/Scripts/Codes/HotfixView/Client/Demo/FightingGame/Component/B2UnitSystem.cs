using System.Numerics;

namespace ET.Client
{
    [FriendOf(typeof(b2Unit))]
    public static class B2UnitSystem
    {
        public class B2UnitAwakeSystem : AwakeSystem<b2Unit,long>
        {
            protected override void Awake(b2Unit self,long unitId)
            {
                self.unitId = unitId;
                EventSystem.Instance.Invoke(new CreateB2bodyCallback(){instanceId = self.unitId});
            }
        }
        
        public class B2UnitLoadSystem : LoadSystem<b2Unit>
        {
            protected override void Load(b2Unit self)
            {
                self.Init();
                //b2World创建刚体
                EventSystem.Instance.Invoke(new CreateB2bodyCallback(){instanceId = self.unitId});
            }
        }

        public static void Init(this b2Unit self)
        {
            self.keyFrame = null;
            self.CollisionBuffer.Clear();
        }

        public class B2UnitPreStepSystem : PreStepSystem<b2Unit>
        {
            protected override void PreStepUpdate(b2Unit self)
            {
                b2Body body = b2WorldManager.Instance.GetBody(self.unitId);
                body.SetVelocity(self.Velocity * new Vector2(- body.GetFlip(), 1) * (self.Hertz / 60f));
            }
        }
        
        public class B2UnitPostStepSystem : PostStepSystem<b2Unit>
        {
            protected override void PosStepUpdate(b2Unit self)
            {
                //清空碰撞信息缓冲区
                self.CollisionBuffer.Clear();
            }
        }
        
        public static Vector2 GetVelocity(this b2Unit self)
        {
            return self.Velocity;
        }

        public static void SetVelocity(this b2Unit self, Vector2 velocity)
        {
            self.Velocity = velocity;
        }
        
        public static void SetVelocityY(this b2Unit self, float velocityY)
        {
            self.Velocity = new Vector2(self.Velocity.X, velocityY);
        }

        public static void SetVelocityX(this b2Unit self, float velocityX)
        {
            self.Velocity = new Vector2(velocityX, self.Velocity.Y);
        }

        public static int GetHertz(this b2Unit self)
        {
            return self.Hertz;
        }

        public static void SetHertz(this b2Unit self, int hertz)
        {
            self.Hertz = hertz;
        }
    }
}