using Testbed.Abstractions;
using UnityEngine;
using UnityEngine.InputSystem;
using Camera = UnityEngine.Camera;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    [Invoke(BBTimerInvokeType.ScreenShakeTimer)]
    public class ScreenShakeXTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            BBTimerComponent bbTimer = BBTimerManager.Instance.SceneTimer();
            float shakeLength_X = CameraManager.instance.shakeLength_X;
            float shakeLength_Y = CameraManager.instance.shakeLength_Y;
            float frequency = CameraManager.instance.frequency;
            int totalFrame = CameraManager.instance.totalFrame;
            int curFrame = CameraManager.instance.curFrame;
            long timer = CameraManager.instance.timer;
            CameraManager.instance.MainCamera.transform.position = CameraManager.instance.Position + 
                    new Vector3(shakeLength_X * Mathf.Cos(curFrame * frequency) * (curFrame / (float)totalFrame), 
                                shakeLength_Y * Mathf.Sin(curFrame * frequency) * (curFrame / (float)totalFrame), 0);
            
            curFrame--;
            if (curFrame <= 0)
            {
                bbTimer.Remove(ref timer);
                CameraManager.instance.shakeLength_X = 0;
                CameraManager.instance.shakeLength_Y = 0;
                CameraManager.instance.frequency = 0;
                CameraManager.instance.totalFrame = 0;
                CameraManager.instance.curFrame = 0;
                CameraManager.instance.timer = 0;
                CameraManager.instance.MainCamera.transform.position = CameraManager.instance.Position;
                return;
            }
            CameraManager.instance.curFrame = curFrame;
        }
    }
    
    [FriendOf(typeof(CameraManager))]
    public static class CameraManagerSystem
    {
        public class CameraManagerAwakeSystem : AwakeSystem<CameraManager>
        {
            protected override void Awake(CameraManager self)
            {
                CameraManager.instance = self;
                self.Init();
            }
        }

        public class CameraManagerUpdateSystem : UpdateSystem<CameraManager>
        {
            protected override void Update(CameraManager self)
            {
                self.CheckZoom();
                self.CheckResize();
                // self.CheckMouseDown();
                // self.CheckMouseMove();
                self.CheckKeyDown();
            }
        }
        
        public class CameraManagerLoadSystem : LoadSystem<CameraManager>
        {
            protected override void Load(CameraManager self)
            {
                self.Init();
            }
        }

        private static void Init(this CameraManager self)
        {
            self.Position = new Vector3(0, 0, -10);
            self.Difference = Vector3.zero;
            self.Origin = Vector3.zero;
            self.Drag = false;

            self._screenWidth = 0;
            self._screenHeight = 0;
            self.Scroll = Vector3.zero;

            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            sceneTimer.Remove(ref self.timer);
            sceneTimer.Remove(ref self.targetTimer);
            self.shakeLength_X = 0;
            self.shakeLength_Y = 0;
            self.frequency = 0;
            self.totalFrame = 0;
            self.curFrame = 0;
            
            self.MainCamera = Camera.main;
            self.MainCamera.transform.position = self.Position;
        }

        #region Zoom
        private static void CheckZoom(this CameraManager self)
        {
            var scroll = Mouse.current.scroll.ReadValue();
            
            //zoom out
            if (scroll.y < 0)
            {
                if (self.MainCamera.orthographicSize > 1)
                {
                    self.MainCamera.orthographicSize += 1f;
                }
                else
                {
                    self.MainCamera.orthographicSize += 0.1f;
                }

                self.Scroll = scroll;
                ScrollCallback(self.Scroll.x, self.Scroll.y);
            }

            //zoom in
            if (scroll.y > 0)
            {
                if (self.MainCamera.orthographicSize > 1)
                {
                    self.MainCamera.orthographicSize -= 1f;
                }
                else if (self.MainCamera.orthographicSize > 0.2f)
                {
                    self.MainCamera.orthographicSize -= 0.1f;
                }

                self.Scroll = scroll;
                ScrollCallback(scroll.x, scroll.y);
            }
        }

        private static void ScrollCallback(float _, float dy)
        {
            if (dy > 0)
            {
                Global.Camera.Zoom /= 1.1f;
            }
            else
            {
                Global.Camera.Zoom *= 1.1f;
            }
        }
        #endregion

        #region Resize

        private static void CheckResize(this CameraManager self)
        {
            var w = Screen.width;
            var h = Screen.height;
            var mode = Screen.fullScreenMode;
            if (self._screenWidth != w || self._screenHeight != h)
            {
                self._screenWidth = w;
                self._screenHeight = h;
                //URP项目中可能因为渲染顺序的问题,GL渲染被清空
                GL.Viewport(new Rect(0, 0, w, h));
                self.ResizeWindowCallback(w, h, mode);
            }
        }

        private static void ResizeWindowCallback(this CameraManager _, int width, int height, FullScreenMode fullScreenMode)
        {
            Global.Camera.Width = width;
            Global.Camera.Height = height;
            
            UnityTestSettings Settings = Global.Settings as UnityTestSettings;
            Settings.WindowWidth = width;
            Settings.WindowHeight = height;
            Settings.FullScreenMode = fullScreenMode;
        }
        #endregion

        #region MouseControl

        // private static void CheckMouseDown(this CameraManager self)
        // {
        //     var mouse = Mouse.current;
        //     var mousePosition = Mouse.current.position.ReadValue();
        //
        //     //Drag
        //     if (mouse.rightButton.isPressed)
        //     {
        //         self.Difference = self.MainCamera.ScreenToWorldPoint(mousePosition) - self.Position;
        //         if (!self.Drag)
        //         {
        //             self.Drag = true;
        //             self.Origin = self.MainCamera.ScreenToWorldPoint(mousePosition);
        //         }
        //     }
        //     else
        //     {
        //         self.Drag = false;
        //     }
        // }
        //
        // private static void CheckMouseMove(this CameraManager self)
        // {
        //     if (Mouse.current.rightButton.isPressed)
        //     {
        //         var delta = Mouse.current.delta.ReadValue();
        //         Global.Camera.Center.X -= delta.x * 0.05f * Global.Camera.Zoom;
        //         Global.Camera.Center.Y += delta.y * 0.05f * Global.Camera.Zoom;
        //     }
        //
        //     if (self.Drag)
        //     {
        //         self.Position = self.Origin - self.Difference;
        //     }
        // }
        //
        #endregion

        #region KeyControl
        private static void CheckKeyDown(this CameraManager _)
        {
            var key = Keyboard.current;
            //Reload
            if (key.f1Key.wasPressedThisFrame)
            {
                CodeLoader.Instance.LoadHotfix();
                EventSystem.Instance.Load();
                Log.Debug("hot reload success");
            }

            //Paused
            if (key.f2Key.wasPressedThisFrame)
            {
                EventSystem.Instance?.Invoke(new PausedCallback() { Pause = !Global.Settings.Pause });
            }

            //Single Step
            if (key.f3Key.wasPressedThisFrame)
            {
                EventSystem.Instance?.Invoke(new SingleStepCallback(){SingleStep = !Global.Settings.SingleStep});
            }
        }
        
        #endregion
    }
}