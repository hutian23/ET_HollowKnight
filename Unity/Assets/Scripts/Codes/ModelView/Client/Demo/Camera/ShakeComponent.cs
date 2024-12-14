using System;

namespace ET.Client
{
    [ComponentOf]
    public class ShakeComponent : Entity,IAwake,IDestroy, IPostStep
    {
        public long UnitId;
        public int ShakeCnt;
        public int TotalShakeCnt;
        public int ShakeLength;
        public Random Random = new();
    }
}