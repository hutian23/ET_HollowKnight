﻿namespace ET.Client
{
    public class DialogueScriptAttribute: BaseAttribute
    {
    }
    
    [DialogueScript]
    public abstract class DialogueScriptHandler
    {
        public abstract string GetOPType();
        public abstract ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token);
    }
}