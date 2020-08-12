using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Simple Script for getting back to the welcome scene by pressing 'W'
public class GetBackToWelcome : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("Return to Welcome Scene");
            SceneManager.LoadScene("Welcome");
        }
    }
}
