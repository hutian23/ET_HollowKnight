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
                if (match.Success)
                {
                    self.marker_Pointer.TryAdd(match2.Groups[1].Value, i);
                }
                i++;
            }
        }

        public static bool BehaviorCheck(this BehaviorInfo self)
        {
            // bool res = true;
            // foreach (string opline in self.opLines)
            // {
            //     Match match = Regex.Match(opline, @"^\w+");
            //     if (!match.Success)
            //     {
            //         DialogueHelper.ScripMatchError(opline);
            //         return false;
            //     }
            //
            //     BBTriggerHandler handler = DialogueDispatcherComponent.Instance.GetTrigger(match.Value);
            //     BBScriptData data = BBScriptData.Create(opline, 0, 0);
            //     BBParser parser = self.GetParent<BehaviorBuffer>().GetParent<TimelineComponent>().GetComponent<BBParser>();
            //
            //     bool ret = handler.Check(parser, data);
            //     if (ret is false)
            //     {
            //         res = false;
            //         break;
            //     }
            // }

            return false;
        }
    }
}