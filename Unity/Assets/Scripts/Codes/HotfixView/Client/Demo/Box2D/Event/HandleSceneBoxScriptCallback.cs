﻿using System.IO;
using System.Text.RegularExpressions;
using Timeline.Editor;
using UnityEngine;

namespace ET.Client
{
    [Invoke(ProcessType.SceneBoxProcess)]
    [FriendOf(typeof(BBParser))]
    public class HandleSceneBoxScriptCallback : AInvokeHandler<ProcessBBScriptCallback>
    {
        public override void Handle(ProcessBBScriptCallback args)
        {
            GameObject _world = GameObject.Find("_World");
            if (_world == null)
            {
                return;
            }
            SceneBox sceneBox = _world.GetComponent<SceneBox>();
            if (sceneBox == null)
            {
                return;
            }
            
            //1. 初始化
            BBParser bbParser = Root.Instance.Get(args.instanceId) as BBParser;
            bbParser.Init();

            //2. 解析textAsset
            string script = string.Empty;
            if (Define.IsEditor)
            {
                script = File.ReadAllText(sceneBox.Script.GetPath());
            }
            else
            {
                //TODO 打包后...
            }
            if (string.IsNullOrEmpty(script)) return;
            
            //3. 加载opLines
            string[] opLines = script.Split('\n');
            int pointer = 0;
            for (int i = 0; i < opLines.Length; i++)
            {
                string op = opLines[i].Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#'))
                {
                    continue;
                }
                bbParser.OpDict[pointer++] = op;
            }
            
            //4. 记录代码头指针
            pointer = 0;
            while (pointer < bbParser.OpDict.Count)
            {
                string opLine = bbParser.OpDict[pointer];
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(opLine, pattern);
                if (match.Success)
                {
                    bbParser.GroupPointerSet.Add(pointer);
                }
                pointer++;
            }
            
            //4. 解析代码块
            foreach (int index in bbParser.GroupPointerSet)
            {
                DataGroup group = DataGroup.Create();
                // groupName
                string opLine = bbParser.OpDict[index];
                string pattern = @"\[(.*?)\]";
                Match match = Regex.Match(opLine, pattern);
                group.groupName = match.Groups[1].Value;
            
                // groupStartIndex
                group.startIndex = index;
                // group Function Marker
                pointer = index + 1;
                while (pointer < bbParser.OpDict.Count)
                {
                    //执行超出代码块
                    if (bbParser.GroupPointerSet.Contains(pointer)) break;
                    string _opLine = bbParser.OpDict[pointer];
                    //匹配函数指针
                    string _pattern = "@([^:]+)";
                    Match _match = Regex.Match(_opLine, _pattern);
                    if (_match.Success)
                    {
                        group.funcPointers.TryAdd(_match.Groups[1].Value, pointer);
                    }
                
                    //匹配Marker指针
                    string _pattern2 = @"SetMarker:\s+'([^']*)'";
                    Match _match2 = Regex.Match(_opLine, _pattern2);
                    if (_match2.Success)
                    {
                        group.markerPointers.TryAdd(_match2.Groups[1].Value, pointer);
                    }
                    pointer++;
                }
                // group endIndex
                group.endIndex = pointer;
                bbParser.GroupDict.TryAdd(group.groupName, group);
            }
        }
    }
}