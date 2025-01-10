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
                UnitComponent unitComponent = scene.CurrentScene().GetComponent<UnitComponent>();
                Unit etc = unitComponent.AddChild<Unit, int>(1001);
                
                //渲染层
                etc.AddComponent<GameObjectComponent>().GameObject = sceneEtc.gameObject;
                sceneEtc.InstanceId = etc.InstanceId;
                
                //逻辑层
                BBParser parser = etc.AddComponent<BBParser, int>(ProcessType.SceneEtcProcess);
                parser.Start();
            }
            
            await ETTask.CompletedTask;
        }
    }
}