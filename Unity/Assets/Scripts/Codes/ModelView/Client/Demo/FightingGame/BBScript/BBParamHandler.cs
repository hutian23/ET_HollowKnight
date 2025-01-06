namespace ET.Client
{
    public class BBParamAttribute: BaseAttribute
    {
        
    }
    
    [BBParam]
    public abstract class BBParamHandler
    {
        public abstract string GetRefType();

        public abstract string Handle(BBParser parser, string param);
    }
}