using System.Text.RegularExpressions;
using UnityEngine.InputSystem;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ThronesTestTimer)]
    [FriendOf(typeof(BBParser))]
    public class TestTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //检测按键输入
            string keyType = self.GetParam<string>("ThronesTestKeyType");
            bool ret = false;
            switch (keyType)
            {
                case "A":
                    ret = Keyboard.current.aKey.isPressed;
                    break;
            }
            if (!ret) return;

            //回调
            int startIndex = self.GetParam<int>("ThronesTest_StartIndex");
            int endIndex = self.GetParam<int>("ThronesTest_EndIndex");
            self.Cancel();
            self.RegistSubCoroutine(startIndex, endIndex, self.CancellationToken).Coroutine();
        }
    }
    
    [FriendOf(typeof(BBParser))]
    public class ThronesTest_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ThronesTest";
        }

        //ThronesTest: A
        //EndThronesTest:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ThronesTest: (?<KeyType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            // 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndThronesTest:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            // 初始化
            parser.TryRemoveParam("ThronesTest_StartIndex");
            parser.TryRemoveParam("ThronesTest_EndIndex");
            parser.TryRemoveParam("ThronesTestTimer");
            parser.TryRemoveParam("ThronesTestKeyType");
            // 注册变量
            parser.RegistParam("ThronesTest_StartIndex", startIndex);
            parser.RegistParam("ThronesTest_EndIndex", endIndex);
            parser.RegistParam("ThronesTestKeyType", match.Groups["KeyType"].Value);
          
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.ThronesTestTimer, parser);
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}