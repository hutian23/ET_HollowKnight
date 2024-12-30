using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(TimelineScripProcessor))]
    [FriendOf(typeof(BBParser))]
    public class HandleTimelineScriptCallback : AInvokeHandler<TimelineScriptCallback>
    {
        public override void Handle(TimelineScriptCallback args)
        {
            //组件
            TimelineScripProcessor processor = Root.Instance.Get(args.instanceId) as TimelineScripProcessor;
            TimelineComponent timelineComponent = processor.GetParent<TimelineComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            TextAsset asset = timelineComponent.GetTimelinePlayer().BBPlayable.BBScript;

            //1. 初始化
            bbParser.Cancel();

            //2. 解析textAsset
            string[] opLines = asset.text.Split('\n');
            int pointer = 0;
            for (int i = 0; i < opLines.Length; i++)
            {
                string op = opLines[i].Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue;
                bbParser.OpDict[pointer++] = op;
            }

            //3. 记录代码块头指针 
            pointer = 0;
            while (pointer < bbParser.OpDict.Count)
            {
                string _opLine = bbParser.OpDict[pointer];
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(_opLine, pattern);
                if (match.Success)
                {
                    bbParser.GroupDict[match.Groups[1].Value] = pointer;
                }
                pointer++;
            }
            
            //4. RootInit协程(纯同步的!!!!这里不应该有异步逻辑)
            bbParser.Invoke(1, bbParser.CancellationToken).Coroutine();
            
            //TODO: 默认行为
        }
    }
}