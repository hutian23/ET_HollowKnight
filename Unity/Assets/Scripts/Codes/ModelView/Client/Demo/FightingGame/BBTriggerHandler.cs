﻿namespace ET.Client
{
    public class BBTriggerAttribute: BaseAttribute
    {
    }

    [BBTrigger]
    public abstract class BBTriggerHandler
    {
        public abstract string GetTriggerType();
    }
}