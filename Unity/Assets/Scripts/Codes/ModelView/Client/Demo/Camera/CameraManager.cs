using UnityEngine;
using Random = System.Random;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class CameraManager : Entity,IAwake,IDestroy, ILoad, IUpdate, ILateUpdate
    {
        [StaticField]
        public static CameraManager instance;
        public Camera MainCamera;
        
        //初始位置
        public Vector3 Position = new(0, 0, -10f);
        //MouseMove
        public Vector3 Difference;
        public Vector3 Origin;
        public bool Drag;

        public int _screenWidth;
        public int _screenHeight;
        
        //Zoom
        public Vector2 Scroll;
        
        //Shake
        public int shakeLength;
        public int shakeCnt;
        public int shakeTotalCnt;
        public Random Random = new();
    }
}