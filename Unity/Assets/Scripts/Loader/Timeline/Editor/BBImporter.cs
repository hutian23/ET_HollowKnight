using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Timeline.Editor
{
    //为.bb文件写一个解析接口
    [ScriptedImporter(1, ".bb")]
    public class BBImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            //作为string读取
            var bbTxt = File.ReadAllText(ctx.assetPath);

            Debug.Log("Import:" + ctx.assetPath);
            
            var assetText = new TextAsset(bbTxt);

            ctx.AddObjectToAsset("main obj", assetText);
            ctx.SetMainObject(assetText);
        }
    }
}