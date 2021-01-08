using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

using UnityEngine;
using UnityEngine.Networking;

public class RESTrequester
{
    private string response;

    public string GetResponse()
    {
        return response;
    }
    
    public IEnumerator MoodleRESTrequest(string ip, Dictionary<string, string> postdata)
    {
        UnityWebRequest request = UnityWebRequest.Post(ip +"webservice/rest/server.php",postdata);

        yield return request.SendWebRequest();

        if (request.isNetworkError) // Error
        {
            Debug.Log(request.error);
        }
        else // Success
        {
            response = request.downloadHandler.text;
        }
    }
}
