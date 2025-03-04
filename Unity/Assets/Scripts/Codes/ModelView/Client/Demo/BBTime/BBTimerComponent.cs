﻿using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Serializable]
    public class BBTimerAction
    {
        public long Id;
        public TimerClass TimerClass;
        public long startFrame;
        public object Object;
        public long Frame; // 持续的帧数
        public int Type; //战斗中的事件 具体见BBTimeInvokeType

        public static BBTimerAction Create(long id, TimerClass timerClass, long startFrame, long frame, int type, object obj)
        {
            BBTimerAction timerAction = ObjectPool.Instance.Fetch<BBTimerAction>();
            timerAction.Id = id;
            timerAction.TimerClass = timerClass;
            timerAction.startFrame = startFrame;
            timerAction.Object = obj;
            timerAction.Type = type;
            timerAction.Frame = frame;
            return timerAction;
        }

        public void Recycle()
        {
            this.Id = 0;
            this.TimerClass = TimerClass.None;
            this.Object = null;
            this.startFrame = 0;
            this.Frame = 0;
            this.Type = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }

    public struct BBTimerCallback
    {
        public object Args;
    }
    
    [ChildOf]
    [ComponentOf]
    public class BBTimerComponent: Entity, IAwake, IDestroy, IUpdate, ILoad
    {
        public readonly MultiMap<long, long> TimerId = new();

        public readonly Queue<long> timeOutTime = new();

        public readonly Queue<long> timeOutTimerIds = new();

        public readonly Dictionary<long, BBTimerAction> timerActions = new();

        public long idGenerator;

        // 记录最小事件，不用每次都去MultiMap取第一个值
        public long minFrame = long.MaxValue;
        public long curFrame = 0;
        
        //标准更新频率60fps
        public int Hertz = 60;
        public long Accumulator;
        
        //BBTimerManager组件管理，每帧都会更新
        //一些特殊的Timer，比如SceneTimer, PostStepTimer, PreStepTimer,更新逻辑跟其父组件有关，不需要BBTimerManager管理
        public bool IsFrameUpdate;
    }
}