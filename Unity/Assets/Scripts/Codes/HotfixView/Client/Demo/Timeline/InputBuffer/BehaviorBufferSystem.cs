namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    [FriendOf(typeof (BehaviorBuffer))]
    [FriendOf(typeof (BBTimerComponent))]
    public static class BehaviorBufferSystem
    {
        [Invoke(BBTimerInvokeType.BehaviorCheckTimer)]
        [FriendOf(typeof (BehaviorBuffer))]
        [FriendOf(typeof (BehaviorInfo))]
        public class BehaviorCheckTimer: BBTimer<BehaviorBuffer>
        {
            protected override void Run(BehaviorBuffer self)
            {
                foreach (long infoId in self.infoIds)
                {
                    BehaviorInfo info = self.GetChild<BehaviorInfo>(infoId);
                    //已经进入当前行为，不会重复检查进入条件
                    //比当前行为权值小的行为也不会进行检查
                    if (info.behaviorOrder == self.currentOrder)
                    {
                        break;
                    }
                    
                    bool ret = info.BehaviorCheck();
                    if (ret)
                    {
                        if (self.currentOrder != info.behaviorOrder)
                        {
                            self.GetParent<TimelineComponent>().Reload(info.Timeline, info.behaviorOrder);
                        }
                    
                        break;
                    }
                }
            }
        }
        
        public static void Init(this BehaviorBuffer self)
        {
            //销毁组件
            self.behaviorNameMap.Clear();
            self.behaviorOrderMap.Clear();
            foreach (long id in self.infoIds)
            {
                self.RemoveChild(id);
            }
            self.infoIds.Clear();
            self.GCOptions.Clear();
            self.ClearParam();
            self.currentOrder = -1;
            self.CheckTimer = 0;
        }

        public static void SetCurrentOrder(this BehaviorBuffer self, int order)
        {
            self.currentOrder = order;
        }

        public static int GetCurrentOrder(this BehaviorBuffer self)
        {
            return self.currentOrder;
        }

        public static BehaviorInfo GetInfoByOrder(this BehaviorBuffer self, int behaviorOrder)
        {
            if (!self.behaviorOrderMap.TryGetValue(behaviorOrder, out long infoId))
            {
                Log.Error($"does not exist behavior, Order: {behaviorOrder}");
                return null;
            }
            return self.GetChild<BehaviorInfo>(infoId);
        }

        public static BehaviorInfo GetInfoByName(this BehaviorBuffer self, string behaviorName)
        {
            if (!self.behaviorNameMap.TryGetValue(behaviorName, out long infoId))
            {
                Log.Error($"does not exist behavior, Name: {behaviorName}");
                return null;
            }
            return self.GetChild<BehaviorInfo>(infoId);
        }
        
        #region Param

        public static T RegistParam<T>(this BehaviorBuffer self, string paramName, T value)
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

        public static T GetParam<T>(this BehaviorBuffer self, string paramName)
        {
            if (!self.paramDict.TryGetValue(paramName, out SharedVariable variable))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            if (variable.value is not T value)
            {
                Log.Error($"cannot format {variable.name} to {typeof (T)}");
                return default;
            }

            return value;
        }

        public static bool ContainParam(this BehaviorBuffer self, string paramName)
        {
            return self.paramDict.ContainsKey(paramName);
        }

        public static bool RemoveParam(this BehaviorBuffer self, string paramName)
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

        public static bool TryRemoveParam(this BehaviorBuffer self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                return false;
            }
            
            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
            return true;
        }
        
        public static void ClearParam(this BehaviorBuffer self)
        {
            foreach (var kv in self.paramDict)
            {
                self.paramDict[kv.Key].Recycle();
            }

            self.paramDict.Clear();
        }
        
        #endregion

        #region GCOption

        public static void AddGCOption(this BehaviorBuffer self, string behaviorName)
        {
            if (!self.behaviorNameMap.TryGetValue(behaviorName, out long infoId))
            {
                Log.Error($"does not exist behavior, Name:{behaviorName}");
                return;
            }

            BehaviorInfo info = self.GetChild<BehaviorInfo>(infoId);
            self.GCOptions.Add(info.behaviorOrder);
        }

        public static bool ContainGCOption(this BehaviorBuffer self, int behaviorOrder)
        {
            return self.GCOptions.Contains(behaviorOrder);
        }

        public static bool ContainGCOption(this BehaviorBuffer self, string behaviorName)
        {
            if (!self.behaviorNameMap.TryGetValue(behaviorName, out long infoId))
            {
                Log.Error($"does not exist behavior, Name:{behaviorName}");
                return false;
            }

            BehaviorInfo info = self.GetChild<BehaviorInfo>(infoId);
            return self.ContainGCOption(info.behaviorOrder);
        }

        #endregion
    }
}