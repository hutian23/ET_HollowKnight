namespace ET.Client
{
    public class BBInputAttribute: BaseAttribute
    {
    }

    [BBInput]
    public abstract class BBInputHandler
    {
        public abstract string GetHandlerType();
        public abstract string GetBufferType();

        public abstract ETTask<InputBuffer> Handle(InputWait self, ETCancellationToken token);
    }
}