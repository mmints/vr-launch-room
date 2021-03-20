using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    private string _url = "http://141.26.140.219:666/";
    

    
    public void GetLevelJson(string levelName)
    {
        StartCoroutine(GetRequest(_url + "/json/" + levelName + ".json", (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.LogError($"{req.error}: {req.downloadHandler.text}");
            } else
            {
                Debug.Log(req.downloadHandler.text);
                
                // The issue seems to be that there are exactly 3 extra bytes on the head of the response.
                // The fix is to use req.bytes instead of req.text, then slice off the extra 3 bytes.
                // From: https://forum.unity.com/threads/jsonutility-fromjson-error-invalid-value.421291/
                string jsonString;
                jsonString = System.Text.Encoding.UTF8.
                    GetString(req.downloadHandler.data, 3, req.downloadHandler.data.Length - 3);

                Level level = new Level(jsonString);
                Debug.Log("Level Display Name: " + level.displayName);
            }
            
        }));
    }
    
    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }
}
