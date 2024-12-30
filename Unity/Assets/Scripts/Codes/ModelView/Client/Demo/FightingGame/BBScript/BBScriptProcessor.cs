using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(TimelineComponent))]
    public class BBScripProcessor : Entity, IAwake,IDestroy, ILoad
    {
        public Dictionary<int, string> opDict = new();
        
        // 用[GroupName]分割代码块。并记录代码块的头指针
        public Dictionary<string,int> groupDict = new();
    }

    public struct ReloadBBScriptCallback
    {
        public long instanceId;
    }
}