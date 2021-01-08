using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Moodle
{
    public class TokenRetriever
    {
        public UserToken Token { get; private set; }

        public IEnumerator RequestToken(string username, string password, string moodleIP, string serviceName)
        {
            Dictionary<string, string> postdata = new Dictionary<string, string>();
            postdata.Add("username", username);
            postdata.Add("password", password);
            postdata.Add("service", serviceName);
            
            UnityWebRequest request = UnityWebRequest.Post(moodleIP + "login/token.php", postdata);
            yield return request.SendWebRequest();
            
            if (request.isNetworkError) // Error
            {
                Debug.Log(request.error);
            }
            else // Success
            {
                Token = JsonUtility.FromJson<UserToken>(request.downloadHandler.text);
            }
        }
    }
}