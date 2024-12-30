using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline
{
    public class Test : MonoBehaviour
    {
        public TextAsset textAsset;
        
        [Button("测试")]
        public void BBTest()
        {
            string[] opLine = this.textAsset.text.Split('\n');
            int pointer = 0;
            Dictionary<int, string> opDict = new();
            
            for (int i = 0; i < opLine.Length; i++)
            {
                string op = opLine[i].Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue;
                opDict[pointer++] = op;
            }

            int j = 0;
            while (j < opDict.Count)
            {
                string _opLine = opDict[j];
                
                //匹配行为的头指针
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(_opLine, pattern);
                if (match.Success)
                {
                    Debug.LogWarning(match.Groups[1].Value+"  "+j);
                }

                j++;
            }
        }
    }
}