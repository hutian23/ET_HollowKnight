using System.Collections.Generic;
using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    public static class BehaviorInfoSystem
    {
        public class SkillInfoDestroySystem: DestroySystem<BehaviorInfo>
        {
            protected override void Destroy(BehaviorInfo self)
            {
                self.behaviorName = string.Empty;
                self.behaviorOrder = 0;
                self.moveType = MoveType.None;
                self.Timeline = null;
                self.opDict.Clear();
                self.function_Pointer.Clear();
                self.marker_Pointer.Clear();
            }
        }

        public static void LoadSkillInfo(this BehaviorInfo self, BBTimeline timeline)
        {
            string opLines = timeline.Script;
            
            //1. split op
            string[] ops = opLines.Split("\n");
            int pointer = 0;
            foreach (string opLine in ops)
            {
                string op = opLine.Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue;
                self.opDict[pointer++] = op;
            }
            
            //2. 缓存指针
            int i = 0;
            while (i < self.opDict.Count)
            {
                string opLine = self.opDict[i];
                
                //匹配函数指针
                string pattern = "@([^:]+)";
                Match match = Regex.Match(opLine, pattern);
                if (match.Success)
                {
                    self.function_Pointer.TryAdd(match.Groups[1].Value, i);
                }
                
                //匹配Marker指针
                string pattern2 = @"SetMarker:\s+'([^']*)'";
                Match match2 = Regex.Match(opLine, pattern2);
                if (match2.Success)
                {
                    self.marker_Pointer.TryAdd(match2.Groups[1].Value, i);
                }
                i++;
            }
        }

        public static bool BehaviorCheck(this BehaviorInfo self)
        {
            int index = self.GetFunctionPointer("Trigger");
            if (index == -1) return false;
            
            bool res = true;
            for (int i = index + 1; i < self.opDict.Count; i++)
            {
                string opLine = self.opDict[i];
                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    DialogueHelper.ScripMatchError(opLine);
                    return false;
                }
                if (match.Value.Equals("return")) break;
                
                BBTriggerHandler handler = ScriptDispatcherComponent.Instance.GetTrigger(match.Value);
                BBScriptData data = BBScriptData.Create(opLine, 0, 0);
                BBParser parser = self.GetParent<BehaviorBuffer>().GetParent<TimelineComponent>().GetComponent<BBParser>();
                
                res = handler.Check(parser, data);
                if (res is false) break;
            }
            
            return res;
        }

        public static int GetFunctionPointer(this BehaviorInfo self, string functionName)
        {
            return self.function_Pointer.GetValueOrDefault(functionName, -1);
        }

        public static int GetMarker(this BehaviorInfo self, string markerName)
        {
            if (!self.marker_Pointer.TryGetValue(markerName, out int index))
            {
                Log.Error($"does not exist marker pointer: {markerName}");
                return -1;
            }

            return index;
        }
    }
}