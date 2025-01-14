﻿using MongoDB.Bson.Serialization.Attributes;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class DialogueStorageManager: Entity, IAwake, IDestroy, ISerializeToEntity,ILoad, IDeserialize
    {
        [BsonIgnore]
        [StaticField]
        public static DialogueStorageManager Instance;

        [BsonIgnore]
        //快照的最大存储量,第一位是快照,不显示在UI中
        public const int MaxSize = 31;

        public long[] shots = new long[MaxSize];

        [BsonIgnore]
        public DialogueStorage QuickSaveShot => this.GetChild<DialogueStorage>(shots[0]);
    }
}