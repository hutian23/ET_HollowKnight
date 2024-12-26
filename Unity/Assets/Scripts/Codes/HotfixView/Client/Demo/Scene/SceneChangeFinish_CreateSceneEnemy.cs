using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(EnemyManager))]
    public class SceneChangeFinish_CreateSceneEnemy : AEvent<SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            GameObject _Enemy = GameObject.Find("_Enemy");
            if (_Enemy == null)
            {
                return;
            }

            //场景内怪物管理单例
            scene.CurrentScene().AddComponent<EnemyManager>();

            //生成怪物unit
            UnitComponent unitComponent = scene.CurrentScene().GetComponent<UnitComponent>();
            foreach (SceneEnemy sceneEnemy in _Enemy.GetComponentsInChildren<SceneEnemy>())
            {
                Unit enemy = unitComponent.AddChild<Unit, int>(1001);
                enemy.AddComponent<GameObjectComponent>().GameObject = sceneEnemy.gameObject;
                sceneEnemy.gameObject.transform.SetParent(GlobalComponent.Instance.Unit);
                sceneEnemy.instanceId = enemy.InstanceId;
                
                TimelineComponent timelineComponent = enemy.AddComponent<TimelineComponent>();
                timelineComponent.AddComponent<BBTimerComponent>().IsFrameUpdateTimer();
                timelineComponent.AddComponent<b2Unit>();
                timelineComponent.AddComponent<ObjectWait>();
                timelineComponent.AddComponent<BBParser>();
                timelineComponent.AddComponent<BehaviorBuffer>();

                EnemyManager.Instance.InstanceIds.Add(enemy.InstanceId);
            }

            await ETTask.CompletedTask;
        }
    }
}