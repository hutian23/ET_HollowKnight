using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 输入模块
    /// </summary>
    [ComponentOf(typeof (TimelineComponent))]
    public class InputWait: Entity, IAwake, IDestroy, IFrameUpdate
    {
        public long curOP;
        public const int MaxStack = 100;
        public Queue<InputInfo> infoQueue = new();
        public List<InputInfo> infoList = new(); //将队列转成List
        public Queue<string> handleQueue = new();
        
        public bool BufferFlag; // 设置输入缓冲区是否启动
        public Dictionary<string, long> BufferDict = new(); // 记录了输入缓冲有效的最大帧号
        public Dictionary<long, long> PressedDict = new();
        public Dictionary<long, long> PressingDict = new();
    }

    public struct InputInfo
    {
        public long frame;
        public long op;
        public int Flip;
    }
    
    public class InputCallback
    {
        public bool IsDisposed
        {
            get
            {
                return tcs == null;
            }
        }

        public ETTask<WaitInput> Task => tcs;

        public void SetResult(WaitInput wait)
        {
            var t = tcs;
            tcs = null;
            t.SetResult(wait);
        }

        public void SetException()
        {
            var t = tcs;
            tcs = null;
            t.SetResult(new WaitInput() { Error = WaitTypeError.Destroy });
        }

        //Param
        public long OP;
        public int waitType;
        public Func<bool> checkFunc;
        public long timeOut;

        private ETTask<WaitInput> tcs;

        public static InputCallback Create(long OP, int waitType, Func<bool> checkFunc,long timeOut)
        {
            InputCallback callback = ObjectPool.Instance.Fetch<InputCallback>();
            callback.OP = OP;
            callback.waitType = waitType;
            callback.tcs = ETTask<WaitInput>.Create(true);
            callback.checkFunc = checkFunc;
            callback.timeOut = timeOut;
            return callback;
        }

        public void Recycle()
        {
            OP = 0;
            waitType = 0;
            tcs = null;
            checkFunc = null;
            timeOut = -1;
            ObjectPool.Instance.Recycle(this);
        }
    }
    
    public struct InputBuffer
    {
        public long curFrame; //添加到缓冲区时的帧号
        public long buffFrame;
        public Status ret;
        
        [StaticField]
        public static InputBuffer None = new() { curFrame = -1, buffFrame = -1, ret = Status.Failed };
    }

    public struct WaitInput: IWaitType
    {
        //缓冲完成条件那一帧的输入
        //把按键都抽象成0和1，按下LP 1,没按下 0
        //把所有输入的状态码拼在一起，变成一个64位的整形，如下
        //eg 0000 0000 0000 0000 0000 0100 0000 0100 当前帧按下LP和下
        public long frame;
        public long OP;
        public int Error { get; set; }
    }

    public static class FuzzyInputType
    {
        public const int None = 0;
        public const int AND = 1;
        public const int OR = 2;
        public const int Hold = 3;
        public const int IsPressed = 4; //长按一个按键，只会执行一次技能
    }

    public static class BBOperaType
    {
        public const int None = 0;
        public const int DOWNLEFT = 2 << 0;
        public const int DOWN = 2 << 1;
        public const int DOWNRIGHT = 2 << 2;
        public const int LEFT = 2 << 3;
        public const int MIDDLE = 2 << 4;
        public const int RIGHT = 2 << 5;
        public const int UPLEFT = 2 << 6;
        public const int UP = 2 << 7;
        public const int UPRIGHT = 2 << 8;

        public const int X = 2 << 9;
        public const int A = 2 << 10;
        public const int Y = 2 << 11;
        public const int B = 2 << 12;
        public const int RB = 2 << 13;
        public const int RT = 2 << 14;
        public const int LB = 2 << 15;
        public const int LT = 2 << 16;
    }
}