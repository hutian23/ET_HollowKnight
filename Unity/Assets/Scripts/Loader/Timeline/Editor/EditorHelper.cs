using System.IO;
using UnityEditor;
using UnityEngine;

namespace Timeline.Editor
{
    public static class EditorHelper
    {
        public static string GetPath(this TextAsset asset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            return Path.GetFullPath(assetPath);
        }
    }
}