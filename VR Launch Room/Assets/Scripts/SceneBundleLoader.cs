﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneBundleLoader : MonoBehaviour
{
    public IEnumerator Start()
    {
        // Download compressed Scene. If version 5 of the file named "Streamed-Level1.unity3d" was previously downloaded and cached.
        // Then Unity will completely skip the download and load the decompressed Scene directly from disk.
        
        // var download = UnityWebRequestAssetBundle.GetAssetBundle("http://141.26.178.164:666/Streamed-level.unity3d");
        var download = UnityWebRequestAssetBundle.GetAssetBundle("http://141.26.140.219:666/Streamed-level.unity3d");
        // var download = UnityWebRequestAssetBundle.GetAssetBundle("http://141.26.178.164:666/scenes");

        // Both variations works!

        yield return download.SendWebRequest();

        // Handle error
        if (download.isNetworkError || download.isHttpError)
        {
            Debug.LogError(download.error);
            yield break;
        }

        // In order to make the Scene available from LoadLevel, we have to load the asset bundle.
        // The AssetBundle class also lets you force unload all assets and file storage once it is no longer needed.
        var bundle = DownloadHandlerAssetBundle.GetContent(download);
    }

    public void LoadScene()
    {
        // Load the level we have just downloaded
        SceneManager.LoadScene("Dummy");
        var scene = SceneManager.GetSceneByName("Dummy");
        Debug.Log("Load scene: " + scene.name);
    }
}