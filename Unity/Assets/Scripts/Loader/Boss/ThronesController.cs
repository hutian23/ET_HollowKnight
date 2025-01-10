using UnityEngine;

namespace ET
{
    public class ThronesController : SceneEtc
    {
        public TextAsset Script;
        
        [HideInInspector]
        public long InstanceId;

        public override void SceneChangeFinish()
        {
            EventSystem.Instance.Invoke(new ThronesControllerCallback() { controller = this });
        }
    }

    public struct ThronesControllerCallback
    {
        public ThronesController controller;
    }
}