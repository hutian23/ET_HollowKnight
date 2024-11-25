namespace ET.Client
{
    public class BBParamAttribute: BaseAttribute
    {
        
    }
    
    //BBParser访问其他组件上的变量
    //Domain表示Parser组件
    [BBParam]
    public abstract class BBParamHandler
    {
        public abstract string getParamType();

        public abstract string Handle(string opLine);
    }
}