using UnityEngine;

namespace ET
{
    public class ThroneController: MonoBehaviour
    {
        public TextAsset Script;

        [HideInInspector]
        public long instanceId;
        
        public void Start()
        {
            instanceId = EventSystem.Instance.Invoke<ThroneControllerCallback, long>(new ThroneControllerCallback(){Script = Script});
        }
    }

    public struct ThroneControllerCallback
    {
        public TextAsset Script;
    }
}