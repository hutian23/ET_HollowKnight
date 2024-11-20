namespace ET.Client
{
    [ChildOf]
    public class BBCallback : Entity,IAwake,IDestroy
    {
        public long CheckTimer;
        public string callBackName;
        public string triggerType;
        public string trigger;
        public int startIndex;
        public int endIndex;
    }
}