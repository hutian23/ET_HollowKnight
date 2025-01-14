﻿using System.Collections.Generic;
using Timeline;

namespace ET.Client
{
    [ChildOf(typeof (BehaviorBuffer))]
    public class BehaviorInfo: Entity, IAwake, IDestroy
    {
        public BBTimeline Timeline;
        public string behaviorName;
        public int behaviorOrder;
        public MoveType moveType;
    }

    public enum MoveType
    {
        None = 0,
        Transition = 1,
        Move = 2,
        Normal = 3,
        Special = 4,
        Super = 5,
        HitStun = 6,
        Etc = 7
    }
}