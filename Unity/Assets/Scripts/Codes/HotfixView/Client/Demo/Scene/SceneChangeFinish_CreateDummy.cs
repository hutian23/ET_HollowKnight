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
            // for (int i = 0; i < 10; i++)
            // {
            //     await ResourcesComponent.Instance.LoadBundleAsync($"dummy.unity3d");
            //     GameObject prefab = (GameObject)ResourcesComponent.Instance.GetAsset($"dummy.unity3d", $"Dummy");
            //     GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            //     go.name = "Dummy";
            //
            //     //2. create dummy unit
            //     UnitComponent unitComponent = scene.CurrentScene().GetComponent<UnitComponent>();
            //     Unit dummy = unitComponent.AddChild<Unit, int>(1005);
            //
            //     //3. dummy component
            //     dummy.AddComponent<ObjectWait>();
            //     dummy.AddComponent<GameObjectComponent>().GameObject = go;
            //
            //     TimelineComponent timelineComponent = dummy.AddComponent<TimelineComponent>();
            //
            //     timelineComponent.AddComponent<BBTimerComponent>().IsFrameUpdateTimer();
            //     timelineComponent.AddComponent<HitboxComponent>();
            //     timelineComponent.AddComponent<ObjectWait>();
            //     timelineComponent.AddComponent<BBParser>();
            //     timelineComponent.AddComponent<BehaviorBuffer>();
            // }
            await ETTask.CompletedTask;
        }
    }
}