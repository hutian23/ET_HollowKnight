using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (DialogueDispatcherComponent))]
    [FriendOf(typeof (DialogueComponent))]
    public static class BBParserSystem
    {
        public class BBParserDestroySystem: DestroySystem<BBParser>
        {
            protected override void Destroy(BBParser self)
            {
                self.Cancel();
                self.EntityId = 0;
            }
        }

        /// <summary>
        /// 取消主协程以及其子协程
        /// </summary>
        public static void Cancel(this BBParser self)
        {
            self.cancellationToken?.Cancel();
            self.funcMap.Clear();
            self.opLines = null;
            self.opDict.Clear();
            self.markers.Clear();
            self.function_Pointers.Clear();

            //回收变量
            foreach (var kv in self.paramDict)
            {
                kv.Value.Recycle();
            }
            self.paramDict.Clear();
            
            //回收callback
            foreach (var kv in self.callBackDict)
            {
                long id = kv.Value.Id;
                self.RemoveChild(id);
            }
            self.callBackDict.Clear();
        }
        
        public static void InitScript(this BBParser self, string script)
        {
            self.Cancel();
            self.opLines = script;

            //热重载取消所有BBParser子协程
            self.cancellationToken = new ETCancellationToken();

            //建立执行语句和指针的映射
            string[] opLines = self.opLines.Split("\n");
            int pointer = 0;
            foreach (string opLine in opLines)
            {
                string op = opLine.Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue; //空行 or 注释行
                self.opDict[pointer++] = op;
            }

            foreach (var kv in self.opDict)
            {
                //函数指针
                string pattern = "@([^:]+)";
                Match match = Regex.Match(kv.Value, pattern);
                if (match.Success)
                {
                    self.funcMap.TryAdd(match.Groups[1].Value, kv.Key);
                }

                //匹配marker
                string pattern2 = @"SetMarker:\s+'([^']*)'";
                Match match2 = Regex.Match(kv.Value, pattern2);
                if (match2.Success)
                {
                    self.markers.TryAdd(match2.Groups[1].Value, kv.Key);
                }
            }
        }

        public static async ETTask<Status> Main(this BBParser self)
        {
            Status ret = await self.Invoke("Main", self.cancellationToken);
            return ret;
        }

        public static int GetMarker(this BBParser self, string markerName)
        {
            if (self.markers.TryGetValue(markerName, out int index))
            {
                return index;
            }

            Log.Error($"not found marker: {markerName}");
            return -1;
        }

        /// <summary>
        /// 同步调用 Main函数或者在Main函数中调用函数
        /// 异步调用 不需要记录指针
        /// </summary>
        public static async ETTask<Status> Invoke(this BBParser self, string funcName, ETCancellationToken token)
        {
            //1. 找到函数入口指针
            if (!self.funcMap.TryGetValue(funcName, out int index))
            {
                Log.Warning($"not found function : {funcName}");
                return Status.Failed;
            }

            //2. 当前协程唯一标识符,生成协程ID和调用指针的映射关系
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            self.function_Pointers.Add(funcId, index);

            //3. 逐条执行语句
            while (++self.function_Pointers[funcId] < self.opDict.Count)
            {
                if (token.IsCancel()) return Status.Failed;

                //4. 语句(OPType: xxxx;) 根据 OPType 匹配handler
                string opLine = self.opDict[self.function_Pointers[funcId]];
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }

                string opType = match.Value;
                if (opType == "SetMarker") continue; //Init时执行过，跳过

                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    return Status.Failed;
                }

                //5. 执行一条语句相当于一个子协程
                BBScriptData data = BBScriptData.Create(self.ReplaceParam(opLine), funcId, null); //池化，不然GC很高
                Status ret = await handler.Handle(self, data, token);
                data.Recycle();

                if (token.IsCancel() || ret == Status.Failed) return Status.Failed;
                if (ret != Status.Success) return ret;
            }

            return Status.Success;
        }
        
        public static async ETTask<Status> RegistSubCoroutine(this BBParser self, int startIndex, int endIndex, string funcName)
        {
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            self.function_Pointers.Add(funcId, startIndex);

            while (++self.function_Pointers[funcId] < endIndex)
            {
                if (self.cancellationToken.IsCancel()) return Status.Failed;
                
                //1. 根据 opType 匹配 handler
                string opLine = self.opDict[self.function_Pointers[funcId]];
                
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }
                
                string opType = match.Value;
                if (opType == "SetMarker") continue;
                
                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler: {opType}");
                    return Status.Failed;
                }
                
                //2. 执行语句
                BBScriptData data = BBScriptData.Create(self.ReplaceParam(opLine), funcId, null);
                Status ret = await handler.Handle(self, data, self.cancellationToken);
                data.Recycle();
                
                if (self.cancellationToken.IsCancel() || ret == Status.Failed) return Status.Failed;
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
                foreach (var param in self.paramDict)
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
            if (self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params:{paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.paramDict.Add(paramName, variable);
            return value;
        }

        public static void UpdateParam<T>(this BBParser self, string paramName, T value)
        {
            foreach ((string key, SharedVariable variable) in self.paramDict)
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
            if (!self.paramDict.TryGetValue(paramName, out SharedVariable variable))
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
            return self.paramDict.ContainsKey(paramName);
        }
        
        public static void RemoveParam(this BBParser self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return;
            }

            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
        }
        
        public static bool TryRemoveParam(this BBParser self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                return false;
            }
            
            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
            return true;
        }

        public static void SetEntityId(this BBParser self, long instanceId)
        {
            self.EntityId = instanceId;
        }

        public static long GetEntityId(this BBParser self)
        {
            return self.EntityId;
        }
    }
}