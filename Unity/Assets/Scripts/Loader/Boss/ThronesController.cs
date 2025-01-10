using UnityEngine;

namespace ET
{
    public class ThronesController : MonoBehaviour
    {
        public TextAsset Script;
        
        [HideInInspector]
        public long InstanceId;
        
        public void Start()
        {
            EventSystem.Instance.Invoke(new ThronesControllerCallback() { controller = this });
        }
    }

    public struct ThronesControllerCallback
    {
        public ThronesController controller;
    }
}