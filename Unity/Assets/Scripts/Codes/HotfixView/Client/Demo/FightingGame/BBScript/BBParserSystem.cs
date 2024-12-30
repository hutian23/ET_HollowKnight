using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (ScriptDispatcherComponent))]
    [FriendOf(typeof (DialogueComponent))]
    public static class BBParserSystem
    {
        public class BBParserDestroySystem: DestroySystem<BBParser>
        {
            protected override void Destroy(BBParser self)
            {
                self.Cancel();
            }
        }

        /// <summary>
        /// 取消主协程以及其子协程
        /// </summary>
        public static void Cancel(this BBParser self)
        {
            self.OpDict.Clear();
            self.GroupDict.Clear();
            self.CancellationToken?.Cancel();
            self.Coroutine_Pointers.Clear();
            //回收变量
            foreach (var kv in self.ParamDict)
            {
                kv.Value.Recycle();
            }
            self.ParamDict.Clear();
        }
        
        public static void Init(this BBParser self)
        {
            self.Cancel();
            self.CancellationToken = new ETCancellationToken();
        }
        
        /// <summary>
        /// 传入函数的头指针
        /// </summary>
        public static async ETTask<Status> Invoke(this BBParser self, int index, ETCancellationToken token)
        {
            //2. 当前协程唯一标识符,生成协程ID和调用指针的映射关系
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            self.Coroutine_Pointers.Add(funcId, index);

            //3. 逐条执行语句
            while (++self.Coroutine_Pointers[funcId] < self.OpDict.Count)
            {
                //4. 语句(OPType: xxxx;) 根据 OPType 匹配handler
                string opLine = self.OpDict[self.Coroutine_Pointers[funcId]];
                //5. 因为用[GroupName]分割代码块，说明指针超出代码块了
                if (opLine.StartsWith('['))
                {
                    return Status.Failed;
                }
                //6. 匹配opType
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }
                string opType = match.Value;
                if (!ScriptDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    return Status.Failed;
                }

                //7. 执行当前指针的语句
                BBScriptData data = BBScriptData.Create(self.ReplaceParam(opLine), funcId, null); //池化，不然GC很高
                Status ret = await handler.Handle(self, data, token);
                data.Recycle();
                
                //8. 协程中断(热重载、unit被销毁了....)
                if (token.IsCancel())
                {
                    return ret;
                }
            }

            return Status.Success;
        }
        
        /// <summary>
        ///  以子协程的形式执行代码块
        /// </summary>
        public static async ETTask<Status> RegistSubCoroutine(this BBParser self, int startIndex, int endIndex, ETCancellationToken token)
        {
            //生成协程Id
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            self.Coroutine_Pointers.Add(funcId, startIndex);
            
            //热重载时销毁子协程
            self.CancellationToken.Add(token.Cancel);
            
            while (++self.Coroutine_Pointers[funcId] < endIndex)
            {
                if (token.IsCancel()) return Status.Failed;
                
                //1. 根据 opType 匹配 handler
                string opLine = self.OpDict[self.Coroutine_Pointers[funcId]];
                
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }
                
                string opType = match.Value;
                if (!ScriptDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler: {opType}");
                    return Status.Failed;
                }
                
                //2. 执行语句
                BBScriptData data = BBScriptData.Create(self.ReplaceParam(opLine), funcId, null);
                Status ret = await handler.Handle(self, data, token);
                data.Recycle();
                
                if (token.IsCancel() || ret == Status.Failed) return Status.Failed;
                if (ret != Status.Success) return ret;
            }
            return Status.Success;
        }

        private static string ReplaceParam(this BBParser self, string opLine)
        {
            MatchCollection matches = Regex.Matches(opLine, @"\[(.*?)\]");
            string result = opLine;
            for (int i = 0; i < matches.Count; i++)
            {
                string content = matches[i].Groups[1].Value;
                string replace = string.Empty;
                //find param
                foreach (var param in self.ParamDict)
                {
                    if (param.Key.Equals(content))
                    {
                       replace = param.Value.value.ToString();
                    }
                }

                result = result.Replace(matches[i].Value, replace);
            }
            return result;
        }
        
        public static T RegistParam<T>(this BBParser self, string paramName, T value)
        {
            if (self.ParamDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params:{paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.ParamDict.Add(paramName, variable);
            return value;
        }

        public static void UpdateParam<T>(this BBParser self, string paramName, T value)
        {
            foreach ((string key, SharedVariable variable) in self.ParamDict)
            {
                if (!key.Equals(paramName))
                {
                    continue;
                }

                variable.value = value;
                return;
            }
            
            Log.Error($"does not exist param:{paramName}!");
        }
        
        public static T GetParam<T>(this BBParser self, string paramName)
        {
            if (!self.ParamDict.TryGetValue(paramName, out SharedVariable variable))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            if (variable.value is not T value)
            {
                Log.Error($"cannot format {variable.name} to {typeof (T)}");
                return default;
            }

            return value;
        }

        public static bool ContainParam(this BBParser self, string paramName)
        {
            return self.ParamDict.ContainsKey(paramName);
        }
        
        public static void RemoveParam(this BBParser self, string paramName)
        {
            if (!self.ParamDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return;
            }

            self.ParamDict[paramName].Recycle();
            self.ParamDict.Remove(paramName);
        }
        
        public static bool TryRemoveParam(this BBParser self, string paramName)
        {
            if (!self.ParamDict.ContainsKey(paramName))
            {
                return false;
            }
            
            self.ParamDict[paramName].Recycle();
            self.ParamDict.Remove(paramName);
            return true;
        }

        public static int GetGroupIndex(this BBParser self, string groupName)
        {
            return self.GroupDict.GetValueOrDefault(groupName, -1);
        }
    }
}