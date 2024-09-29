﻿namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class GameManager: Entity, IAwake, IDestroy, ILoad, IUpdate
    {
        [StaticField]
        public static GameManager Instance;
    }
    
}