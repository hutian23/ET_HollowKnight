﻿using System.Linq;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (BBTimerComponent))]
    public static class BBTimerComponentSystem
    {
        [Invoke]
        public class BBTimerComponentChangedCallback: AInvokeHandler<BBTimeChanged>
        {
            public override void Handle(BBTimeChanged args)
            {
                BBTimerComponent timerComponent = Root.Instance.Get(args.instanceId) as BBTimerComponent;
                timerComponent.SetTimeScale(args.timeScale);
            }
        }

        public class TODTimerComponentUpdateSystem: UpdateSystem<BBTimerComponent>
        {
            protected override void Update(BBTimerComponent self)
            {
                self.TimerUpdate();
            }
        }

        public class TODTimerComponentDestorySystem: DestroySystem<BBTimerComponent>
        {
            protected override void Destroy(BBTimerComponent self)
            {
                self.Init();
            }
        }

        private static long GetId(this BBTimerComponent self)
        {
            return ++self.idGenerator;
        }

        public static long GetNow(this BBTimerComponent self)
        {
            return self.curFrame;
        }

        private static void Init(this BBTimerComponent self)
        {
            //回收所有定时器
            foreach (var action in self.timerActions.Values)
            {
                action?.Recycle();
            }

            self.TimerId.Clear();
            self.timeOutTime.Clear();
            self.timeOutTimerIds.Clear();
            self.timerActions.Clear();

            self.timeScale = 1f;
            self.minFrame = long.MaxValue;
            self.curFrame = 0;
            self.deltaTimereminder = 0f;
        }

        /// <summary>
        /// 获得一帧的真实时长
        /// </summary>
        private static float GetFrameLength(this BBTimerComponent self)
        {
            //假设一秒为60帧
            return Mathf.Round(1000 / (60 * self.timeScale));
        }

        private static void TimerUpdate(this BBTimerComponent self)
        {
            //时间完全静止了
            if (self.timeScale == 0)
            {
                return;
            }

            self.deltaTimereminder += Time.deltaTime * 1000;

            float frameLength = self.GetFrameLength();
            int num = (int)(self.deltaTimereminder / frameLength);
            self.deltaTimereminder -= num * frameLength;
            self.curFrame += num;

            //当前帧没有可执行的定时器，就不进行遍历了
            if (self.curFrame < self.minFrame)
            {
                return;
            }

            foreach (long k in self.TimerId.Select(kv => kv.Key))
            {
                // 设置定时器中的最小执行帧号
                if (k > self.curFrame)
                {
                    self.minFrame = k;
                    break;
                }

                self.timeOutTime.Enqueue(k);
            }

            while (self.timeOutTime.Count > 0)
            {
                long time = self.timeOutTime.Dequeue();
                var list = self.TimerId[time];
                for (int i = 0; i < list.Count; i++)
                {
                    long timerId = list[i];
                    self.timeOutTimerIds.Enqueue(timerId);
                }

                self.TimerId.Remove(time);
            }

            while (self.timeOutTimerIds.Count > 0)
            {
                long timerId = self.timeOutTimerIds.Dequeue();

                if (!self.timerActions.Remove(timerId, out BBTimerAction timerAction))
                {
                    continue;
                }

                self.Run(timerAction);
            }
        }

        private static void Run(this BBTimerComponent self, BBTimerAction timerAction)
        {
            switch (timerAction.TimerClass)
            {
                case TimerClass.OnceTimer:
                    EventSystem.Instance.Invoke(timerAction.Type, new BBTimerCallback() { Args = timerAction.Object });
                    timerAction.Recycle();
                    break;
                case TimerClass.OnceWaitTimer:
                {
                    ETTask tcs = timerAction.Object as ETTask;
                    tcs.SetResult();
                    timerAction.Recycle();
                    break;
                }
                case TimerClass.RepeatedTimer:
                {
                    timerAction.startFrame = self.curFrame;
                    self.AddTimer(timerAction);
                    EventSystem.Instance.Invoke(timerAction.Type, new BBTimerCallback() { Args = timerAction.Object });
                    break;
                }
            }
        }

        private static void AddTimer(this BBTimerComponent self, BBTimerAction timer)
        {
            long tillFrame = timer.startFrame + timer.Frame;
            self.TimerId.Add(tillFrame, timer.Id);
            self.timerActions.Add(timer.Id, timer);

            if (tillFrame < self.minFrame)
            {
                self.minFrame = tillFrame;
            }
        }

        public static bool Remove(this BBTimerComponent self, ref long id)
        {
            long i = id;
            id = 0;
            return self.Remove(i);
        }

        private static bool Remove(this BBTimerComponent self, long id)
        {
            if (id == 0)
            {
                return false;
            }

            if (!self.timerActions.Remove(id, out BBTimerAction timerAction))
            {
                return false;
            }

            timerAction.Recycle();
            return true;
        }

        public static async ETTask WaitTillAsync(this BBTimerComponent self, long tillFrame, ETCancellationToken token = null)
        {
            // 传入的帧号要比当前的小
            if (self.curFrame >= tillFrame)
            {
                return;
            }

            ETTask tcs = ETTask.Create(true);
            //将回调传入ETTask,Upate中取出 并执行回调
            BBTimerAction timer = BBTimerAction.Create(self.GetId(),
                TimerClass.OnceWaitTimer,
                self.curFrame,
                tillFrame - self.curFrame,
                0,
                tcs);

            self.AddTimer(timer);
            long timerId = timer.Id;

            void CancelAction()
            {
                if (self.Remove(timerId))
                {
                    tcs.SetResult();
                }
            }

            try
            {
                token?.Add(CancelAction);
                await tcs;
            }
            finally
            {
                token?.Remove(CancelAction);
            }
        }

        public static async ETTask WaitAsync(this BBTimerComponent self, long frame, ETCancellationToken token)
        {
            if (frame == 0)
            {
                return;
            }

            //从对象池中取出ETTask
            ETTask tcs = ETTask.Create(true);
            BBTimerAction timer = BBTimerAction.Create(self.GetId(), TimerClass.OnceWaitTimer, self.curFrame, frame, 0, tcs);
            self.AddTimer(timer);
            long timerId = timer.Id;

            void CancelAction()
            {
                if (self.Remove(timerId))
                {
                    tcs.SetResult();
                }
            }

            try
            {
                token?.Add(CancelAction);
                await tcs;
            }
            finally
            {
                token?.Remove(CancelAction);
            }
        }

        public static async ETTask WaitFrameAsync(this BBTimerComponent self, ETCancellationToken token = null)
        {
            await self.WaitAsync(1, token);
        }

        public static long NewOnceTimer(this BBTimerComponent self, long tillFrame, int type, object args)
        {
            if (tillFrame < self.curFrame)
            {
                Log.Error($"tillframe should be bigger than currentFrame:{tillFrame} {self.curFrame}");
            }

            BBTimerAction timer = BBTimerAction.Create(self.GetId(), TimerClass.OnceTimer, self.curFrame, tillFrame - self.curFrame, type, args);
            self.AddTimer(timer);
            return timer.Id;
        }

        public static long NewFrameTimer(this BBTimerComponent self, int type, object args)
        {
            return self.NewRepeatedTimer(1, type, args);
        }

        private static long NewRepeatedTimer(this BBTimerComponent self, long frame, int type, object args)
        {
            BBTimerAction timer = BBTimerAction.Create(self.GetId(), TimerClass.RepeatedTimer, self.curFrame, frame, type, args);

            self.AddTimer(timer);
            return timer.Id;
        }

        public static void SetTimeScale(this BBTimerComponent self, float timeScale)
        {
            self.timeScale = timeScale;
        }

        public static float GetTimeScale(this BBTimerComponent self)
        {
            return self.timeScale;
        }
    }
}