﻿namespace ET.Client
{
    public class Return_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "return";
        }

        //return;
        public override async ETTask<Status> Handle(Unit unit, ScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}