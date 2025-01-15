using UnityEngine;

namespace ET
{
    public class BBScript : MonoBehaviour
    {
        //unit.instanceId, 生成currentScene后，会根据BBScript生成对应的unit并挂载BBParser组件
        [HideInInspector]
        public long instanceId;
        public TextAsset Script;
    }
}
