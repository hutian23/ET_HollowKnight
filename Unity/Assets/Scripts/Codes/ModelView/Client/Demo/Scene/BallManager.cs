using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BallManager : Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static BallManager Instance;
        
        public Queue<long> InstanceIds = new();
    }
}