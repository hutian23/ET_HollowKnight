using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class TextAssetLoader : Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static TextAssetLoader Instance;
        //path ---> text
        public Dictionary<string, string> textAssetDict = new();

        public HashSet<string> assetSet = new();
    }
}