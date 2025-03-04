﻿namespace ET
{
    public enum InstanceQueueIndex
    {
        None = -1,
        Update,
        LateUpdate,
        Load,
#if !DOTNET
        FixedUpdate,
        PreStep,
        PostStep,
        FrameUpdate,
        FrameLateUpdate,
        GizmosUpdate,
#endif
        Max,
    }
}