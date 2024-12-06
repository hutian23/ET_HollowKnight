using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (BehaviorInfo))]
    public class BBExecutable: Entity, IAwake, IDestroy
    {
        public Dictionary<string, int> function_Pointers = new();
        public Dictionary<string, int> marker_Pointers = new();
        public Dictionary<int, string> opDict = new();
        public string opLines;
    }
}