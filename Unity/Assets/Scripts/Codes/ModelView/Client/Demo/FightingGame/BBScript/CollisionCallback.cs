using ET.Event;

namespace ET.Client
{
    public class CollisionCallbackAttribute: BaseAttribute
    {
    }
    
    [CollisionCallback]
    public abstract class CollisionCallback
    {
        public abstract string GetCollisionType();

        //检查回调是否能执行，如果执行成功，移除回调
        public abstract bool Handle(BBParser parser, CollisionInfo info);
    }
}