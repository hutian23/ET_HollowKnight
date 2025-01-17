﻿using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 该组件用于管理行为切换
    /// </summary>
    [ComponentOf]
    public class BehaviorMachine: Entity, IAwake, IDestroy
    {
        //当前行为
        public int currentOrder;
        public Dictionary<string, SharedVariable> paramDict = new();
        public Dictionary<string, long> behaviorNameMap = new();
        public Dictionary<int, long> behaviorOrderMap = new();
        public SortedSet<long> DescendInfoList = new(Comparer<long>.Create((x, y) => y.CompareTo(x))); //方便倒序获取行为信息组件
        public Dictionary<string, long> behaviorFlagDict = new();

        public ETCancellationToken Token = new(); // 热更新时，负责初始化行为机执行中注册的变量
    }

    #region 行为机相关事件
    
    public struct BeforeBehaviorReloadCallback
    {
        public long instanceId;
    }

    public struct LandCallback
    {
        public long instanceId;
    }
    #endregion
}