using System.IO;
using UnityEditor;

namespace Editor
{
    public class CreateAssetBundles : UnityEditor.Editor
    {
        [MenuItem("Build/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            string assetBundleDirectory = "Assets/AssetBundles";
        
            if(!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        }
    }
}    