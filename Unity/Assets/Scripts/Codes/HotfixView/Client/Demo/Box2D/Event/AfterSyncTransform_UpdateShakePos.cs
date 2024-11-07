using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterSyncTransform_UpdateShakePos : AEvent<AfterSyncTransform>
    {
        protected override async ETTask Run(Scene scene, AfterSyncTransform args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            
            if (!timelineComponent.ContainParam("Shake")) return;
            
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            Vector3 shake = timelineComponent.GetParam<Vector2>("Shake");
            go.transform.position += shake;
            
            await ETTask.CompletedTask;
        }
    }
}