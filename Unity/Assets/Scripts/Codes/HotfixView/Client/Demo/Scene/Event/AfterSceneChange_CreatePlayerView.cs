﻿using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterSceneChange_CreatePlayerView: AEvent<CreatePlayerView>
    {
        protected override async ETTask Run(Scene scene, CreatePlayerView args)
        {
            Unit player = TODUnitHelper.GetPlayer(scene.ClientScene());

            //1. 加载AB
            await ResourcesComponent.Instance.LoadBundleAsync($"{player.Config.ABName}.unity3d");
            
            GameObject prefab = (GameObject)ResourcesComponent.Instance.GetAsset($"{player.Config.ABName}.unity3d", $"{player.Config.Name}");
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);

            //2. 以下组件 切换场景时全部销毁
            player.AddComponent<GameObjectComponent>().GameObject = go;
            player.AddComponent<ObjectWait>();
            
            //3. Timeline
            TimelineComponent timelineComponent = player.AddComponent<TimelineComponent>();
            
            // 战斗相关的计时器(因为和角色行为逻辑关联性强，作为timeline的组件)
            timelineComponent.AddComponent<BBTimerComponent>().IsFrameUpdateTimer();
            timelineComponent.AddComponent<b2Unit,long>(player.InstanceId);
            timelineComponent.AddComponent<InputWait>();
            timelineComponent.AddComponent<ObjectWait>();
            timelineComponent.AddComponent<BehaviorBuffer>();
            timelineComponent.AddComponent<BBParser, int>(ProcessType.TimelineProcess);
            timelineComponent.GetComponent<BBParser>().Start();
        }
    }
}