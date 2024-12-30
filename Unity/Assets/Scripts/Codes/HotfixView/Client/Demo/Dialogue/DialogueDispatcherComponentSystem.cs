using System;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public static class DialogueDispatcherComponentSystem
    {
        public class DialogueDispatcherComponentAwakeSystem: AwakeSystem<ScriptDispatcherComponent>
        {
            protected override void Awake(ScriptDispatcherComponent self)
            {
                ScriptDispatcherComponent.Instance = self;
                self.Init();
            }
        }

        public class DialogueDispatcherComponentLoadSystem: LoadSystem<ScriptDispatcherComponent>
        {
            protected override void Load(ScriptDispatcherComponent self)
            {
                self.Init();
            }
        }

        public class DialogueDispatcherComponentDestroySystem: DestroySystem<ScriptDispatcherComponent>
        {
            protected override void Destroy(ScriptDispatcherComponent self)
            {
                self.dispatchHandlers.Clear();
                ScriptDispatcherComponent.Instance = null;
            }
        }

        private static void Init(this ScriptDispatcherComponent self)
        {
            self.dispatchHandlers.Clear();
            var nodeHandlers = EventSystem.Instance.GetTypes(typeof (DialogueAttribute));
            foreach (Type type in nodeHandlers)
            {
                NodeHandler nodeHandler = Activator.CreateInstance(type) as NodeHandler;
                if (nodeHandler == null)
                {
                    Log.Error($"this nodeHandler is not nodeHandler!: {type.Name}");
                    continue;
                }

                self.dispatchHandlers.Add(nodeHandler.GetDialogueType(), nodeHandler);
            }

            self.checker_dispatchHandlers.Clear();
            var nodeCheckerHandlers = EventSystem.Instance.GetTypes(typeof (NodeCheckerAttribute));
            foreach (Type type in nodeCheckerHandlers)
            {
                NodeCheckHandler nodeCheckHandler = Activator.CreateInstance(type) as NodeCheckHandler;
                if (nodeCheckHandler == null)
                {
                    Log.Error($"this obj is not a nodeCheckerHandler:{type.Name}");
                    continue;
                }

                self.checker_dispatchHandlers.Add(nodeCheckHandler.GetNodeCheckType(), nodeCheckHandler);
            }

            self.scriptHandlers.Clear();
            var scriptHandlers = EventSystem.Instance.GetTypes(typeof (DialogueScriptAttribute));
            foreach (Type type in scriptHandlers)
            {
                DialogueScriptHandler handler = Activator.CreateInstance(type) as DialogueScriptHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a ScriptableHandler: {type.Name}");
                    continue;
                }

                self.scriptHandlers.Add(handler.GetOPType(), handler);
            }

            self.replaceHandlers.Clear();
            var replaceHandlers = EventSystem.Instance.GetTypes(typeof (DialogueReplaceAttribute));
            foreach (var type in replaceHandlers)
            {
                ReplaceHandler handler = Activator.CreateInstance(type) as ReplaceHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a replaceHandler: {type.Name}");
                    continue;
                }

                self.replaceHandlers.Add(handler.GetReplaceType(), handler);
            }

            self.BBCheckHandlers.Clear();
            var bbHandlers = EventSystem.Instance.GetTypes(typeof (BBScriptCheckAttribute));
            foreach (var checker in bbHandlers)
            {
                BBCheckHandler handler = Activator.CreateInstance(checker) as BBCheckHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a BBCheckerHandler: {checker.Name}");
                    continue;
                }

                self.BBCheckHandlers.Add(handler.GetBehaviorType(), handler);
            }

            self.BBScriptHandlers.Clear();
            var bbScriptHandlers = EventSystem.Instance.GetTypes(typeof (BBScriptAttribute));
            foreach (var bbScript in bbScriptHandlers)
            {
                BBScriptHandler handler = Activator.CreateInstance(bbScript) as BBScriptHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a bbScriptHandler:{bbScript.Name} ");
                    continue;
                }

                self.BBScriptHandlers.Add(handler.GetOPType(), handler);
            }

            self.BBTriggerHandlers.Clear();
            var bbTriggerHandlers = EventSystem.Instance.GetTypes(typeof (BBTriggerAttribute));
            foreach (var bbTrigger in bbTriggerHandlers)
            {
                BBTriggerHandler handler = Activator.CreateInstance(bbTrigger) as BBTriggerHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a bbTriggerHandler:{bbTrigger.Name}");
                    continue;
                }
                
                self.BBTriggerHandlers.Add(handler.GetTriggerType(), handler);
            }
            
            self.InputHandlers.Clear();
            var inputHandlers = EventSystem.Instance.GetTypes(typeof (InputAttribute));
            foreach (var inputHandler in inputHandlers)
            {
                InputHandler handler = Activator.CreateInstance(inputHandler) as InputHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a inputHandler: {inputHandler.Name}");
                    continue;
                }

                self.InputHandlers.Add(handler.GetHandlerType(), handler);
            } 
        }

        public static async ETTask<Status> Handle(this ScriptDispatcherComponent self, Unit unit, object node, ETCancellationToken token)
        {
            if (self.dispatchHandlers.TryGetValue(node.GetType(), out NodeHandler handler))
            {
                //执行脚本
                await ScriptDispatcherComponent.Instance.ScriptHandles(unit, node as DialogueNode, token);
                if (token.IsCancel())
                {
                    return Status.Failed;
                }
                return await handler.Handle(unit, node, token);
            }

            Log.Error($"not found handler: {node}");
            return Status.Failed;
        }

        private static int Check(this ScriptDispatcherComponent self, Unit unit, NodeCheckConfig nodeCheck)
        {
            if (!self.checker_dispatchHandlers.TryGetValue(nodeCheck.GetType(), out NodeCheckHandler nodeCheckerHandler))
            {
                throw new Exception($"not found nodeCheckerHandler: {nodeCheck}");
            }

            return nodeCheckerHandler.Check(unit, nodeCheck);
        }

        public static int Checks(this ScriptDispatcherComponent self, Unit unit, List<NodeCheckConfig> nodeCheckList)
        {
            if (nodeCheckList == null) return 0;
            foreach (var nodeCheck in nodeCheckList)
            {
                if (self.Check(unit, nodeCheck) != 0)
                {
                    return 1;
                }
            }

            return 0;
        }

        public static string GetReplaceStr(this ScriptDispatcherComponent self, Unit unit, string replaceType, string replaceText)
        {
            if (self.replaceHandlers.TryGetValue(replaceType, out ReplaceHandler handler))
            {
                return handler.GetReplaceStr(unit, replaceText);
            }

            return string.Empty;
        }

        public static BBCheckHandler GetBBCheckHandler(this ScriptDispatcherComponent self, string name)
        {
            if (self.BBCheckHandlers.TryGetValue(name, out BBCheckHandler handler))
            {
                return handler;
            }

            return null;
        }

        public static BBTriggerHandler GetTrigger(this ScriptDispatcherComponent self, string name)
        {
            if (!self.BBTriggerHandlers.TryGetValue(name, out BBTriggerHandler handler))
            {
                Log.Error($"not found triggerHandler: {name}");
                return null;
            }

            return handler;
        }

        public static InputHandler GetInputHandler(this ScriptDispatcherComponent self, string name)
        {
            if (!self.InputHandlers.TryGetValue(name, out InputHandler handler))
            {
                Log.Error($"not found inputHandler: {name}");
                return null;
            }

            return handler;
        }
    }
}