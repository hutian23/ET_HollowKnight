using System.Numerics;

namespace ET.Client
{
    [ChildOf(typeof(VirtualCamera))]
    public class CameraTarget : Entity, IAwake, IDestroy
    {
        public string Name;
        public long UnitId;
    }
}