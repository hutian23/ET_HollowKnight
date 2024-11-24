using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class b2GameManager: Entity, IAwake, IDestroy, IFixedUpdate, IPostStep
    {
        [StaticField]
        public static b2GameManager Instance;
        
        public b2Game Game;
        public b2World B2World;
        public Dictionary<long, long> BodyDict = new();
        public bool Paused = false;

        //物理模拟相关的生命周期函数
        public long PreStepTimer;
        public long PostStepTimer;
    }
}