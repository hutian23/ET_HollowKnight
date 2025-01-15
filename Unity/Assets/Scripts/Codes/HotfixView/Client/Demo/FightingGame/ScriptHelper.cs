﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(DialogueComponent))]
    [FriendOf(typeof(ScriptDispatcherComponent))]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BBParser))]
    public static class ScriptHelper
    {
        public static void ScripMatchError(string text)
        {
            Log.Error($"{text}匹配失败！请检查格式");
        }

        public static void Reload()
        {
            CodeLoader.Instance.LoadHotfix();
            EventSystem.Instance.Load();
            Log.Debug("hot reload success");
        }

        public static bool Trigger(this BehaviorInfo self)
        {
            BBParser parser = self.GetParent<BehaviorMachine>().GetParent<Unit>().GetComponent<BBParser>();

            //不存在函数
            int index = parser.GetFunctionPointer(self.behaviorName, "Trigger");
            if (index < 0)
            {
                return false;
            }

            for (int i = index + 1; i < parser.OpDict.Count; i++)
            {
                string opLine = parser.OpDict[i];
                if (opLine.Equals("return;"))
                {
                    return true;
                }
                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    ScripMatchError(opLine);
                    return false;
                }
                
                //执行TriggerHandler
                BBTriggerHandler handler = ScriptDispatcherComponent.Instance.GetTrigger(match.Value);
                BBScriptData data = BBScriptData.Create(opLine, 0, 0);
                bool ret = handler.Check(parser, data);
                if (ret is false)
                {
                    return false;
                }
            }
            
            return true;
        }

        public static void Reload(this BehaviorMachine self, string behaviorName)
        {
            BehaviorInfo info = self.GetInfoByName(behaviorName);
            self.Reload(info.behaviorOrder);
        }
        
        public static void Reload(this BehaviorMachine self, int behaviorOrder)
        {
            BBParser parser = self.GetParent<Unit>().GetComponent<BBParser>();
            BehaviorInfo info = self.GetInfoByOrder(behaviorOrder);
            self.SetCurrentOrder(behaviorOrder);
            
            // 切换行为前的回调
            EventSystem.Instance.Invoke(new BeforeBehaviorReloadCallback(){instanceId = self.InstanceId});
            // 执行行为协程
            parser.Invoke(parser.GetFunctionPointer(info.behaviorName,"Main"),parser.CancellationToken).Coroutine();
        }
    }
}