namespace ET.Client
{
    public class BBInputAttribute: BaseAttribute
    {
    }

    [BBInput]
    public abstract class BBInputHandler
    {
        public abstract string GetInputType();

        public abstract ETTask<InputStatus> Handle(Unit unit, ETCancellationToken token);
    }
}