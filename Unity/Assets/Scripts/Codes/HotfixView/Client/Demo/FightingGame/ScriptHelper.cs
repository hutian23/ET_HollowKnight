namespace ET.Client
{
    [FriendOf(typeof (DialogueComponent))]
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public static class ScriptHelper
    {
        public static void ScripMatchError(string text)
        {
            Log.Error($"{text}匹配失败！请检查格式");
        }

        public static void Reload()
        {
            CodeLoader.Instance.LoadHotfix();
            EventSystem.Instance.Load();
            Log.Debug("hot reload success");
        }
    }
}