﻿namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BBTimerComponent))]
    public static class BehaviorMachineSystem
    {
        public class BehaviorBufferAwakeSystem : AwakeSystem<BehaviorMachine>
        {
            protected override void Awake(BehaviorMachine self)
            {
                self.Init();
            }
        }

        private static void Init(this BehaviorMachine self)
        {
            self.ClearParam();
            self.currentOrder = -1;
            self.behaviorOrderMap.Clear();
            self.behaviorNameMap.Clear();
            self.DescendInfoList.Clear();
            self.behaviorFlagDict.Clear();
        }

        public static void SetCurrentOrder(this BehaviorMachine self, int order)
        {
            self.currentOrder = order;
        }

        public static int GetCurrentOrder(this BehaviorMachine self)
        {
            return self.currentOrder;
        }

        public static BehaviorInfo GetInfoByOrder(this BehaviorMachine self, int behaviorOrder)
        {
            if (!self.behaviorOrderMap.TryGetValue(behaviorOrder, out long infoId))
            {
                Log.Error($"does not exist behavior, Order: {behaviorOrder}");
                return null;
            }
            return self.GetChild<BehaviorInfo>(infoId);
        }

        public static BehaviorInfo GetInfoByName(this BehaviorMachine self, string behaviorName)
        {
            if (!self.behaviorNameMap.TryGetValue(behaviorName, out long infoId))
            {
                Log.Error($"does not exist behavior, Name: {behaviorName}");
                return null;
            }
            return self.GetChild<BehaviorInfo>(infoId);
        }

        #region Param

        public static T RegistParam<T>(this BehaviorMachine self, string paramName, T value)
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

        public static T GetParam<T>(this BehaviorMachine self, string paramName)
        {
            if (!self.paramDict.TryGetValue(paramName, out SharedVariable variable))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            if (variable.value is not T value)
            {
                Log.Error($"cannot format {variable.name} to {typeof(T)}");
                return default;
            }

            return value;
        }

        public static bool ContainParam(this BehaviorMachine self, string paramName)
        {
            return self.paramDict.ContainsKey(paramName);
        }

        public static bool RemoveParam(this BehaviorMachine self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return false;
            }

            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
            return true;
        }

        public static bool TryRemoveParam(this BehaviorMachine self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                return false;
            }

            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
            return true;
        }

        public static void ClearParam(this BehaviorMachine self)
        {
            foreach (var kv in self.paramDict)
            {
                self.paramDict[kv.Key].Recycle();
            }

            self.paramDict.Clear();
        }

        #endregion
    }
}