using Timeline;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(TimelineComponent))]
    public static class TimelineComponentSystem
    {
        public class TimelineComponentAwakeSystem : AwakeSystem<TimelineComponent>
        {
            protected override void Awake(TimelineComponent self)
            {
                self.Bind();
            }
        }
        
        public class TimelineComponentDestroySystem : DestroySystem<TimelineComponent>
        {
            protected override void Destroy(TimelineComponent self)
            {
                self.Init();
            }
        }
        
        public class TimelineComponentLoadSystem : LoadSystem<TimelineComponent>
        {
            protected override void Load(TimelineComponent self)
            {
                self.Init();
            }
        }

        /// <summary>
        /// 渲染层传入组件instanceId，方便渲染层回调事件
        /// </summary>
        /// <param name="self"></param>
        private static void Bind(this TimelineComponent self)
        {
            GameObject go = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            TimelinePlayer timelinePlayer = go.GetComponent<TimelinePlayer>();
            if (timelinePlayer == null)
            {
                return;
            }
            timelinePlayer.instanceId = self.InstanceId;
        }

        private static void Init(this TimelineComponent self)
        {
            foreach (var kv in self.markerEventDict)
            {
                TimelineMarkerEvent markerEvent = self.GetChild<TimelineMarkerEvent>(kv.Value);
                markerEvent.Dispose();
            }
            self.markerEventDict.Clear();
            self.SetHertz(60);
        }
        
        #region TimelinePlayer
        public static TimelinePlayer GetTimelinePlayer(this TimelineComponent self)
        {
            return self.GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>();
        }

        public static void Evaluate(this TimelineComponent self, int targetFrame)
        { 
            self.GetTimelinePlayer().RuntimePlayable.Evaluate(targetFrame);
        }

        #endregion
        
        public static TimelineMarkerEvent GetMarkerEvent(this TimelineComponent self, string eventName)
        {
            if (!self.markerEventDict.TryGetValue(eventName, out long id))
            {
                return null;
            }

            return self.GetChild<TimelineMarkerEvent>(id);
        }

        public static void SetHertz(this TimelineComponent self,int hertz)
        {
            EventSystem.Instance.Invoke(new UpdateHertzCallback(){instanceId = self.InstanceId,Hertz = hertz});
        }

        public static int GetHertz(this TimelineComponent self)
        {
            return self.Hertz;
        }
    }
}