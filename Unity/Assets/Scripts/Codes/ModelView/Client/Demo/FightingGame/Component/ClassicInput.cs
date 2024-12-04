using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(InputWait))]
    public class ClassicInput : Entity,IAwake,IDestroy, IFrameUpdate
    {
        public Queue<long> OpQueue = new();
        public const int MaxStack = 30;

        public List<string> classicHandler = new();
    }

    public struct ClassicInputInfo
    {
        public long CurFrame;
        public long OP;
        public FlipState flipState;
    }
}