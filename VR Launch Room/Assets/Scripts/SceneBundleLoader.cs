using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneBundleLoader : MonoBehaviour
{
    public string assetBundleName;

    private UnityWebRequest download;
    
    public IEnumerator Start()
    {
        // Download the requested AssetBundle that contains the Scene
        Debug.Log("Start Download");
        download = UnityWebRequestAssetBundle.GetAssetBundle("http://141.26.140.219:666/" + assetBundleName);
        yield return download.SendWebRequest();
        Debug.Log("End Download");
        // Handle error
        if (download.isNetworkError || download.isHttpError)
        {
            Debug.LogError(download.error);
            yield break;
        }
    }
    
    void Update()
    {
        // Debugging, using keyboard for bundle loading
        if (Input.GetKeyUp(KeyCode.L))
        {
            LoadScene();
        }    
    }

    public void LoadScene() // Will be called by ButtonDown function of the top game object
    {
        DebugCheckForLoadedAssetBundles(1);
        // In order to make the Scene available from LoadLevel, we have to load the asset bundle
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(download);
        Debug.Log("Get Asset Bundle Content.");

        DebugCheckForLoadedAssetBundles(2);
        
        // Get the name of the first Scene like organized in the Building Setting from the
        // AssetBundle's origin Project. Hence, it is the starting Scene.
        string startSceneName = bundle.GetAllScenePaths()[0];
        Debug.Log("Start Scene: " + startSceneName);
        
        SceneManager.LoadScene(startSceneName);
        var scene = SceneManager.GetSceneByName(startSceneName);
        Debug.Log("Load scene: " + scene.name);
        
        DebugCheckForLoadedAssetBundles(3);
        bundle.Unload(true); // Unloads an AssetBundle freeing its data.
        // In either case you won't be able to load any more objects from this bundle unless it is reloaded.
        DebugCheckForLoadedAssetBundles(4);
    }

    // Use this function to figure out if there are any Asset Bundles still loaded
    private void DebugCheckForLoadedAssetBundles(int id)
    {
        Debug.Log("#### "+ id +" ####");
        
        AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();
        Debug.Log("Number of AssetBundles: " + bundles.Length);

        if (bundles.Any())
        {
            foreach (var bundle in bundles)
            {
                Debug.Log(bundle.name);
            }
        }
    }
}