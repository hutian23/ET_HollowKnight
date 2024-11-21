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
        public abstract void Handle(BBParser parser);

        //注册这个回调时，添加依赖的组件,初始化数值等
        public abstract void Regist(BBParser parser);
        //销毁相关组件，重置数值等
        public abstract void Dispose(BBParser parser);
    }
}