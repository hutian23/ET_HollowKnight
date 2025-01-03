namespace ET.Client
{
    [UniqueId(0, 1000)]
    public static class BBTimerInvokeType
    {
        public const int None = 0;
        public const int Test1 = 1;
        public const int CheckInput = 2;

        public const int BehaviorBuffer_TriggerTimer = 3;
        public const int BehaviorBufferCheckTimer = 4;
        public const int BehaviorTimer = 5;
        public const int BehaviorCheckTimer = 6;
        public const int BBInputNotifyTimer = 7;
        public const int BBInputHandleTimer = 8;

        //和行为有关的计时器
        public const int UpdateFlipTimer = 100;
        public const int MoveXTimer = 101;
        public const int GravityCheckTimer = 102;
        public const int AirMoveTimer = 103;
        public const int HitPushBackTimer = 104;
        public const int DefaultWindowTimer = 105;
        public const int WhiffWindowTimer = 106;
        public const int GCWindowTimer = 107;
        public const int CancelWindowTimer = 108;
        public const int TransitionWindowTimer = 109;
        public const int ShakeTimer = 110;
        public const int LoopTimer = 111;
        public const int CallbackCheckTimer = 112;
        public const int AirCheckTimer = 113;
        
        //物理模拟相关的生命周期函数
        public const int HitCheckTimer = 313;
        public const int HurtNotifyTimer = 314;
        public const int ThrowCheckTimer = 315;
        
    }
}