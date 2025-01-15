using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class EnemyManager : Entity, IAwake,IDestroy, ILoad
    {
        [StaticField]
        public static EnemyManager Instance;

        public HashSet<long> InstanceIds = new();

        //子弹之类的unit，热重载时销毁
        public Queue<long> ReloadQueue = new();
    }
}