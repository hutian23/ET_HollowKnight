﻿using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 该组件用于管理行为切换
    /// </summary>
    [ComponentOf(typeof (TimelineComponent))]
    public class BehaviorBuffer: Entity, IAwake, IDestroy
    {
        public long CheckTimer;
        
        //当前行为
        public int currentOrder;

        //共享变量，当前行为中缓存的一些变量，重载行为时会把缓存的共享变量添加到BBParser组件中
        public Dictionary<string, SharedVariable> paramDict = new();
        public HashSet<int> GCOptions = new();
        public HashSet<int> WhiffOptions = new();

        //方便通过behaviorName找到behaviorInfo
        public Dictionary<string, long> behaviorNameMap = new();
        public Dictionary<int, long> behaviorOrderMap = new();
        public SortedSet<long> infoIds = new(Comparer<long>.Create((x, y) => y.CompareTo(x)));
        
        //受击模块
        public Dictionary<string, long> hitStunFlagMap = new();
    }

    public struct WaitHitStunBehavior : IWaitType
    {
        public string hitStunFlag;
        public int Error { get; set; }
    }
}