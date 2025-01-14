﻿using Timeline;
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
            self.ClearParam();
            foreach (var kv in self.callbackDict)
            {
                TimelineCallback callback = self.GetChild<TimelineCallback>(kv.Value);
                callback.Dispose();
            }
            self.callbackDict.Clear();
            
            foreach (var kv in self.markerEventDict)
            {
                TimelineMarkerEvent markerEvent = self.GetChild<TimelineMarkerEvent>(kv.Value);
                markerEvent.Dispose();
            }
            self.markerEventDict.Clear();
            
            self.Token.Cancel();
            self.Token = new ETCancellationToken();
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
        
        #region Param
        public static T RegistParam<T>(this TimelineComponent self, string paramName, T value)
        {
            if (self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params:{paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.paramDict.Add(paramName, variable);
            return value;
        }

        public static T GetParam<T>(this TimelineComponent self, string paramName)
        {
            if (!self.paramDict.TryGetValue(paramName, out SharedVariable variable))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            //类型匹配
            if (variable.value is T value)
            {
                return value;
            }
            
            // 手动支持特定类型的转换
            object rawValue = variable.value;

            if (typeof(T) == typeof(long) && rawValue is int intValue)
            {
                return (T)(object)(long)intValue;
            }
            if (typeof(T) == typeof(double) && rawValue is float floatValue)
            {
                return (T)(object)(double)floatValue;
            }
            if (typeof(T) == typeof(decimal) && rawValue is double doubleValue)
            {
                return (T)(object)(decimal)doubleValue;
            }

            Log.Error($"Cannot cast {variable.name} to {typeof(T)}.");
            return default;
        }

        public static bool ContainParam(this TimelineComponent self, string paramName)
        {
            return self.paramDict.ContainsKey(paramName);
        }

        public static void RemoveParam(this TimelineComponent self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return;
            }

            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
        }

        public static bool TryRemoveParam(this TimelineComponent self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                return false;
            }
            
            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
            return true;
        }

        private static void ClearParam(this TimelineComponent self)
        {
            foreach (var kv in self.paramDict)
            {
                self.paramDict[kv.Key].Recycle();
            }

            self.paramDict.Clear();
        }

        public static void UpdateParam<T>(this TimelineComponent self, string paramName, T value)
        {
            foreach ((string key, SharedVariable variable) in self.paramDict)
            {
                if (!key.Equals(paramName))
                {
                    continue;
                }

                variable.value = value;
                return;
            }
            
            Log.Error($"does not exist param:{paramName}!");
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