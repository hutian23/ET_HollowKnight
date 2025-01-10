using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeFinish_CreateSceneEtc : AEvent<SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            GameObject _Etc = GameObject.Find("_Etc");
            if (_Etc == null)
            {
                return;
            }
            
            foreach (SceneEtc sceneEtc in _Etc.GetComponentsInChildren<SceneEtc>())
            {
                sceneEtc.SceneChangeFinish();
            }
            
            await ETTask.CompletedTask;
        }
    }
}