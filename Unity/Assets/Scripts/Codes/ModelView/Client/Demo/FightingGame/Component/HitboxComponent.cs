using System.Collections.Generic;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class HitboxComponent: Entity, IAwake, IDestroy
    {
        public HitboxKeyframe keyFrame;

        //考虑到回调有注册先后顺序的问题?
        public Queue<string> callbackQueue = new();
    }
}