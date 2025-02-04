namespace ET.Client
{
    public static class VirtualCameraSystem
    {
        public class VirtualCameraAwakeSystem : AwakeSystem<VirtualCamera>
        {
            protected override void Awake(VirtualCamera self)
            {
                VirtualCamera.Instance = self;
            }
        }
        
        public class VirtualCameraDestroySystem : DestroySystem<VirtualCamera>
        {
            protected override void Destroy(VirtualCamera self)
            {
                VirtualCamera.Instance = null;
            }
        }
    }
}