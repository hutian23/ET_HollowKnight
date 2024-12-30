using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class ScriptDispatcherComponent: Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static ScriptDispatcherComponent Instance;

        public Dictionary<Type, NodeHandler> dispatchHandlers = new();

        public Dictionary<Type, NodeCheckHandler> checker_dispatchHandlers = new();

        public Dictionary<string, DialogueScriptHandler> scriptHandlers = new();

        public Dictionary<string, ReplaceHandler> replaceHandlers = new();

        public Dictionary<string, BBCheckHandler> BBCheckHandlers = new();
        public Dictionary<string, BBScriptHandler> BBScriptHandlers = new();
        public Dictionary<string, BBTriggerHandler> BBTriggerHandlers = new();
        public Dictionary<string, InputHandler> InputHandlers = new();
    }
}