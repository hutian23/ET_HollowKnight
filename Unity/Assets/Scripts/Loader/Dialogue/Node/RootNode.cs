﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public class RootNode : DialogueNode
    {
        [FoldoutGroup("$nodeName"),HideInInspector]
        public uint nextNode;
    }
}