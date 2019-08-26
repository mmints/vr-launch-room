using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleLoader : MonoBehaviour
{
    public string assetBundleUrl;
    public string sceneName;
    
    public void LoadSceneForAssetBundle()
    {
        AssetBundle assetBundle = LoadBundle(assetBundleUrl);
        SceneManager.LoadScene(sceneName);
    }
    
    public AssetBundle LoadBundle(string bundleUrl)
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(bundleUrl);
        Debug.Log(assetBundle == null ? "Failed to load Asset Bundle" : "Successfully loaded Asset Bundle");
        return assetBundle;
    }
}
