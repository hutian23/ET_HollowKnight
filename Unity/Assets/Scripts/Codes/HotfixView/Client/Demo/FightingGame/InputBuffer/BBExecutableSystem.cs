using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBExecutable))]
    public static class BBExecutableSystem
    {
        public class BBExecutableDestroySystem : DestroySystem<BBExecutable>
        {
            protected override void Destroy(BBExecutable self)
            {
                self.function_Pointers.Clear();
                self.marker_Pointers.Clear();
                self.opDict.Clear();
                self.opLines = string.Empty;
            }
        }
        
        public static void Parse(this BBExecutable self, string opLines)
        {
            self.opLines = opLines;

            //1. split op
            string[] ops = self.opLines.Split("\n");
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
                    self.function_Pointers.TryAdd(match.Groups[1].Value, i);
                }
                
                //匹配Marker指针
                string pattern2 = @"SetMarker:\s+'([^']*)'";
                Match match2 = Regex.Match(opLine, pattern2);
                if (match.Success)
                {
                    self.marker_Pointers.TryAdd(match2.Groups[1].Value, i);
                }
                
                i++;
            }
        }
    }
}