﻿using System.Collections.Generic;
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
                self.opLines.Clear();
            }
        }

        public static void LoadSkillInfo(this BehaviorInfo self, BBTimeline timeline)
        {
            string[] ops = timeline.Script.Split("\n");
            List<string> trims = new();
            foreach (string opline in ops)
            {
                string op = opline.Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue; //空行 or 注释行
                trims.Add(op);
            }

            //Trigger函数指针
            for (int i = 0; i < trims.Count; i++)
            {
                string pattern = @"^@Trigger:";
                Match match = Regex.Match(trims[i], pattern);
                if (match.Success)
                {
                    for (int j = i + 1; j < trims.Count; j++)
                    {
                        var opline = trims[j];
                        if (opline.Equals("return;"))
                        {
                            break;
                        }

                        self.opLines.Add(opline);
                    }

                    break;
                }
            }
        }

        public static bool SkillCheck(this BehaviorInfo self)
        {
            bool res = true;
            foreach (string opline in self.opLines)
            {
                Match match = Regex.Match(opline, @"^\w+");
                if (!match.Success)
                {
                    DialogueHelper.ScripMatchError(opline);
                    return false;
                }

                BBTriggerHandler handler = DialogueDispatcherComponent.Instance.GetTrigger(match.Value);
                BBScriptData data = BBScriptData.Create(opline, 0, 0);
                BBParser parser = self.GetParent<BehaviorBuffer>().GetParent<TimelineComponent>().GetComponent<BBParser>();

                bool ret = handler.Check(parser, data);
                if (ret is false)
                {
                    res = false;
                    break;
                }
            }

            return res;
        }
    }
}