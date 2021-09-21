using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// This Class manages the connection to the odl4u.ko-ld.de server.
// Level information and the corresponding AssetBundles are
// downloaded and made available to the rest of the software.

public class NetworkManager
{
    // Address to odl4u.ko-ld.de.
    private string _url = "http://141.26.140.219:666/";
    
    private Level _level;
    private AssetBundle _assetBundle;
    
    // Use this inside of a Co-Routine an safe the output in a member variable.
    // Call this always after GetLevelJson!
    public Level GetLevel()
    {
        return _level;
    }

    public AssetBundle GetAssetBundle()
    {
        return _assetBundle;
    }

    public IEnumerator GetLevelRequest(string levelName)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(_url + "json/" + levelName + ".json"))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError($"{request.error}: {request.downloadHandler.text}");
            } else
            {
                Debug.Log(request.downloadHandler.text);
            
                // The issue seems to be that there are exactly 3 extra bytes on the head of the response.
                // The fix is to use req.bytes instead of req.text, then slice off the extra 3 bytes.
                // From: https://forum.unity.com/threads/jsonutility-fromjson-error-invalid-value.421291/
                string jsonString;
                jsonString = System.Text.Encoding.UTF8.
                    GetString(request.downloadHandler.data, 3, request.downloadHandler.data.Length - 3);

                _level = new Level(jsonString);
                Debug.Log("Instantiate Level from Json: " + _level.name);
            }
        }
    }
    
    public IEnumerator GetAssetBundleRequest(string assetBundleName)
    {
        // Use Comment for Caching -> There are download fuck ups when Nils tries to download the asset bundle
        //using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(_url + "asset-bundles/" + assetBundleName,0, 0)) 
        using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(_url + "asset-bundles/" + assetBundleName))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
        
            // Handle error
            if (request.isNetworkError || request.isHttpError) 
            {
                Debug.LogError($"{request.error}: {request.downloadHandler.text}");
            }
            else
            {
                _assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            }
        }
    }
}
