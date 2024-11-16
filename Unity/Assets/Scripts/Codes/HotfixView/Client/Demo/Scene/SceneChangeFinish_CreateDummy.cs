using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeFinish_CreateDummy : AEvent<SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            //1. load dummy go
            Unit player = TODUnitHelper.GetPlayer(scene.ClientScene());
            await ResourcesComponent.Instance.LoadBundleAsync($"{player.Config.ABName}.unity3d");
            GameObject prefab = (GameObject)ResourcesComponent.Instance.GetAsset($"{player.Config.ABName}.unity3d", $"{player.Config.Name}");
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            go.name = "Dummy";
            
            //2. create dummy unit
            UnitComponent unitComponent = scene.CurrentScene().GetComponent<UnitComponent>();
            Unit dummy = unitComponent.AddChild<Unit, int>(player.ConfigId);
            
            //3. dummy component
            dummy.AddComponent<GameObjectComponent>().GameObject = go;
            go.transform.position = new Vector3(3, 5, 0);
            await ETTask.CompletedTask;
        }
    }
}