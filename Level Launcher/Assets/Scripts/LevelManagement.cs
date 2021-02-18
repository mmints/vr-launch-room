using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/* This script manages the processes of the level loading.
 *
 * For now, some parts are hard coded as POC.
 * First, a data base system has to be installed on the odl4u.ko-ld.de server.
 *
 * Functionalities:
 * 1. Downloading of AssetBundles form odl4u.ko-ld.de
 * 2. Caching of AssetBundles 
 * 3. Select Scene from AssetBundle and load it by using SceneLoaderVR.cs
 *
 */

public class LevelManagement : MonoBehaviour
{
    // Connection to the server
    private string serverURL = "http://141.26.140.219:666/";     
    private AssetBundle assetBundle; // The downloaded AssetBundle
    
    public string assetBundleName;
    
    public void Start()
    {
        StartCoroutine(DownloadAssetBundle(assetBundleName));
    }

    // Download the requested asset bundle.
    // If the requested asset bundle was already downloaded, it will be loaded from the cache.
    IEnumerator DownloadAssetBundle(string assetBundleName)
    { 
        UnityWebRequest downloadWebRequest; // Use for HTTP communication
        
        // Download the requested asset bundle or load it from cache by version number
        Debug.Log("Start Download of: " + assetBundleName);
        downloadWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(serverURL + assetBundleName, 0, 0);
        
        // Print the downloading progress
        while (!downloadWebRequest.isDone)
        {
            // TODO: PIPE THE COUNTER TO A PROGRESS BAR
            Debug.Log("Downloading: " + downloadWebRequest.downloadProgress);
        }
        yield return downloadWebRequest.SendWebRequest();
        Debug.Log("End Download");
        
        // Handle error
        if (downloadWebRequest.isNetworkError || downloadWebRequest.isHttpError) 
        {
            // TODO: SHOW A ERROR MESSAGE TO THE USER
            Debug.LogError(downloadWebRequest.error);
        }
        else
        {
            // Store the downloaded asset bundle
            assetBundle = DownloadHandlerAssetBundle.GetContent(downloadWebRequest);
        }
    }

    void GetAllAvailableAssetBundles(string userName)
    {
        // TODO: RETURN A LIST OF STRING OF ALL AVAILABLE ASSET BUNDLE FOR THE LOGGED IN USER  
    }

    
    void GetSceneNameOfAssetBundles()
    {
        // TODO: RETURN THE NAME OF THE SCENE THAT IS STORED IN THE ASSET BUNDLE
    }
}