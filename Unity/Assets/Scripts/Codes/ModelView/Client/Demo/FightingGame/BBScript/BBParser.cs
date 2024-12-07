using System;
using System.Collections.Generic;

namespace ET.Client
{
    //运行时解析BBScript然后执行
    [ComponentOf]
    [ChildOf]
    public class BBParser: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<int, string> opDict = new();
        public ETCancellationToken CancellationToken; // 取消当前执行的所有子协程
        public Dictionary<long, int> Coroutine_Pointers = new(); // 协程ID --> 协程指针
        public Dictionary<string, SharedVariable> ParamDict = new(); // 在携程内注册变量，携程执行完毕dispose
    }
    
    public class BBScriptData
    {
        public string opLine; //指令码
        public long CoroutineID; //协程ID
        public object userData; //数据体

        public static BBScriptData Create(string opLine, long functionID, object userData)
        {
            BBScriptData scriptData = ObjectPool.Instance.Fetch<BBScriptData>();
            scriptData.opLine = opLine;
            scriptData.CoroutineID = functionID;
            scriptData.userData = userData;
            return scriptData;
        }

        public void Recycle()
        {
            opLine = string.Empty;
            this.CoroutineID = 0;
            userData = null;
            ObjectPool.Instance.Recycle(this);
        }
    }
    
    #region If
    public enum SyntaxType
    {
        None,
        Condition,
        Normal
    }

    [Serializable]
    public class SyntaxNode
    {
        public List<SyntaxNode> children = new();
        public SyntaxType nodeType;
        public int index;
        public int endIndex;

        public static SyntaxNode Create(SyntaxType nodeType, int index)
        {
            SyntaxNode node = ObjectPool.Instance.Fetch<SyntaxNode>();
            node.nodeType = nodeType;
            node.index = index;
            return node;
        }

        public void Recycle()
        {
            this.nodeType = SyntaxType.None;
            this.index = 0;
            this.endIndex = 0;
            this.children.Clear();
            ObjectPool.Instance.Recycle(this);
        }
    }
    #endregion
}