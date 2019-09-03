using UnityEditor;
using System.IO;

public class CreateAssetBundles : Editor
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
    
    // Outdated function. Just used for testing.
    [MenuItem("Build/Build SceneBundles")]
    static void BuildAllSceneBundles()
    {
        string assetSceneDirectory = "Assets/SceneBundles/";
        
        if(!Directory.Exists(assetSceneDirectory))
        {
            Directory.CreateDirectory(assetSceneDirectory);
        }

        string[] levels = new string[] {"Assets/Scenes/Dummy.unity"};
        BuildPipeline.BuildStreamedSceneAssetBundle(levels, assetSceneDirectory + "Streamed-level.unity3d", BuildTarget.StandaloneWindows);
    }

}    