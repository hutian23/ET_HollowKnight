using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class EnemyManager : Entity, IAwake,IDestroy
    {
        [StaticField]
        public static EnemyManager Instance;

        public List<long> InstanceIds = new();
    }
}