using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

// This script controls the behavior of the MENU button on the VIVE Controller
public class MenuAction : MonoBehaviour
{
    public string launchRoom; // Scene name of the launch room

    // a reference to the action
    public SteamVR_Action_Boolean openMenu; //Currently just used to get back to the Launch Room
    public SteamVR_Input_Sources handType;
    
    // TODO: Exchange SceneManager.LoadScene by SteamVR_LoadLevel
    private SteamVR_LoadLevel steamVrLoadLevel; // Currently not in use
    
    // Implementation for connecting menu (Player LeftHand) and LaserPointer (Player RightHand)
    // to the MenuButton Action
    public GameObject playerLeftHandMenu;
    public GameObject playerRightHand;
    private SteamVR_LaserPointer instanceOfPlayerLaserPointer;
    
    private void Start()
    {
        openMenu.AddOnStateDownListener(OnMenuButton, handType);
        instanceOfPlayerLaserPointer = playerRightHand.GetComponent<SteamVR_LaserPointer>();
    }
    
    // Load launchRoom Scene when triggered
    public void OnMenuButton(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Menu Button Clicked!");

        if (!playerLeftHandMenu.activeInHierarchy)
            playerLeftHandMenu.SetActive(true);
        else
            playerLeftHandMenu.SetActive(false);

        // TODO: Toggling the LaserPoint makes some problems. The aiming ray does not disappear. 
        if (!instanceOfPlayerLaserPointer.enabled)
            instanceOfPlayerLaserPointer.enabled = true;
        else
            instanceOfPlayerLaserPointer.enabled = false;
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