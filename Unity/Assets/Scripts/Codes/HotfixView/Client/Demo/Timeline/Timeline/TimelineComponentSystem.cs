using Timeline;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(TimelineComponent))]
    public static class TimelineComponentSystem
    {
        [FriendOf(typeof(TimelineManager))]
        public class TimelineComponentAwakeSystem : AwakeSystem<TimelineComponent>
        {
            protected override void Awake(TimelineComponent self)
            {
                //绑定渲染层
                GameObject go = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
                TimelinePlayer timelinePlayer = go.GetComponent<TimelinePlayer>();
                timelinePlayer.instanceId = self.InstanceId;

                //单例管理
                TimelineManager.Instance.instanceIds.Add(self.InstanceId);
            }
        }

        [FriendOf(typeof(TimelineManager))]
        public class TimelineComponentDestroySystem : DestroySystem<TimelineComponent>
        {
            protected override void Destroy(TimelineComponent self)
            {
                self.Init();
                TimelineManager.Instance.instanceIds.Remove(self.InstanceId);
            }
        }

        public static void Init(this TimelineComponent self)
        {
            self.ClearParam();
        }
        
        #region TimelinePlayer

        public static T GetParameter<T>(this TimelineComponent timelineComponent, string parameterName)
        {
            TimelinePlayer timelinePlayer = timelineComponent.GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>();
            BBPlayableGraph playableGraph = timelinePlayer.BBPlayable;
            foreach (SharedVariable param in playableGraph.Parameters)
            {
                if (param.name == parameterName)
                {
                    if (param.value is not T value)
                    {
                        Log.Error($"cannot format {param.name} to {typeof(T)}");
                        return default;
                    }

                    return value;
                }
            }

            return default;
        }

        public static object GetParameter(this TimelineComponent self, string paramName)
        {
            TimelinePlayer timelinePlayer = self.GetTimelinePlayer();

            BBPlayableGraph playableGraph = timelinePlayer.BBPlayable;
            foreach (SharedVariable param in playableGraph.Parameters)
            {
                if (param.name == paramName)
                {
                    return param.value;
                }
            }

            return null;
        }

        public static TimelinePlayer GetTimelinePlayer(this TimelineComponent self)
        {
            return self.GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>();
        }

        public static void Evaluate(this TimelineComponent self, int targetFrame)
        {
            //抛出事件
            EventSystem.Instance.PublishAsync(self.ClientScene().CurrentScene(), new AfterTimelineEvaluated() { instanceId = self.InstanceId, targetFrame = targetFrame }).Coroutine();
            RuntimePlayable playable = self.GetTimelinePlayer().RuntimePlayable;
            playable.Evaluate(targetFrame);
        }

        #endregion

        public static void Reload(this TimelineComponent self, BBTimeline timeline,int behaviorOrder)
        {
            BBParser parser = self.GetComponent<BBParser>();

            //显示层reload playableGraph
            self.GetTimelinePlayer().Init(timeline);
            parser.InitScript(timeline.Script);

            //3. 切换行为前，初始化组件
            EventSystem.Instance.PublishAsync(self.DomainScene(), new BeforeBehaviorReload() { behaviorOrder = behaviorOrder, instanceId = self.GetParent<Unit>().InstanceId }).Coroutine();
            parser.Main().Coroutine();
        }

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
    }
}