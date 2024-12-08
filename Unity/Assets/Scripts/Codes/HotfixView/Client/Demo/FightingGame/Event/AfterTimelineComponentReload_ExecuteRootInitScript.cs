using System.Collections.Generic;

namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(BBParser))]
    public class AfterTimelineComponentReload_ExecuteRootInitScript : AEvent<AfterTimelineComponentReload>
    {
        protected override async ETTask Run(Scene scene, AfterTimelineComponentReload args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            BBParser parser = timelineComponent.GetComponent<BBParser>();
            
            //1. parse Script
            string RootScript = timelineComponent.GetTimelinePlayer().BBPlayable.rootScript;
            Dictionary<int, string> opDict = new();
            string[] ops = RootScript.Split("\n");
            int pointer = 0;
            foreach (string opLine in ops)
            {
                string op = opLine.Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue;
                opDict[pointer++] = opLine;
            }
            
            //2. 
            parser.Init(opDict);
            await parser.Invoke(1, parser.CancellationToken);
        }
    }
}