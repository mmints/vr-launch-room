using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

// This script controls the behavior of the MENU button on the VIVE Controller
public class MenuAction : MonoBehaviour
{
    // a reference to the action
    public SteamVR_Action_Boolean openMenu; //Currently just used to get back to the Launch Room
    private SteamVR_LoadLevel steamVrLoadLevel; // Currently not in use
    // TODO: Exchange SceneManager.LoadScene by SteamVR_LoadLevel
    
    public SteamVR_Input_Sources handType;

    public string launchRoom;
    
    private void Start()
    {
        openMenu.AddOnStateDownListener(OnMenuButton, handType);
    }
    
    // Load launchRoom Scene when triggered
    public void OnMenuButton(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Menu Button Clicked!");
        SceneManager.LoadScene(launchRoom);
    }

    private void Update()
    {
        // Use for debugging and quick switching to the launchRoom by keyboard
        if (Input.GetKeyUp(KeyCode.R))
        {
            Debug.Log("Return to Launch Room!");
            
            // TODO: Exchange SceneManager.LoadScene by SteamVR_LoadLevel
            SceneManager.LoadScene(launchRoom);
        }
    }
}