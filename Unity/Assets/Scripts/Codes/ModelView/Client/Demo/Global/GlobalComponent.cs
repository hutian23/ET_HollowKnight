using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    //GlobalComponent 挂载在rootScene下
    public class GlobalComponent: Entity, IAwake
    {
        [StaticField]
        public static GlobalComponent Instance;

        public Transform Global;
        public Transform Unit { get; set; }

        public Transform NormalRoot { get; set; }
        public Transform PopUpRoot { get; set; }
        public Transform FixedRoot { get; set; }
        public Transform PoolRoot { get; set; }
        public Transform OtherRoot { get; set; }
    }
}