using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class MenuAction : MonoBehaviour
{

    // a reference to the action
    public SteamVR_Action_Boolean openMenu; //Currently just used to get back to the Launch Room
    
    public SteamVR_Input_Sources handType;

    public string launchRoom;
    
    private void Start()
    {
        openMenu.AddOnStateDownListener(OnMenuButton, handType);
    }
    
    public void OnMenuButton(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Menu Button Clicked!");
        SceneManager.LoadScene(launchRoom);
    }
}
