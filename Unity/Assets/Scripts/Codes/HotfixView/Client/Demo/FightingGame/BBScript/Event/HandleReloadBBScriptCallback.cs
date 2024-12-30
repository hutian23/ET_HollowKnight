using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBScripProcessor))]
    public class HandleReloadBBScriptCallback : AInvokeHandler<ReloadBBScriptCallback>
    {
        public override void Handle(ReloadBBScriptCallback args)
        {
            BBScripProcessor processor = Root.Instance.Get(args.instanceId) as BBScripProcessor;
            TimelineComponent timelineComponent = processor.GetParent<TimelineComponent>();
            TextAsset asset = timelineComponent.GetTimelinePlayer().BBPlayable.BBScript;

            //1. 初始化
            processor.opDict.Clear();
            processor.groupDict.Clear();
            
            //2. 解析textAsset
            string[] opLines = asset.text.Split('\n');
            int pointer = 0;
            for (int i = 0; i < opLines.Length; i++)
            {
                string op = opLines[i].Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue;
                processor.opDict[pointer++] = op;
            }
            
            //3. 记录代码块头指针 
            pointer = 0;
            while (pointer < processor.opDict.Count)
            {
                string _opLine = processor.opDict[pointer];
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(_opLine, pattern);
                if (match.Success)
                {
                    processor.groupDict[match.Groups[1].Value] = pointer;
                }
                pointer++;
            }
        }
    }
}