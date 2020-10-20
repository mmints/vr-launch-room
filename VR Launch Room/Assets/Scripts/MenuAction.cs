// DEPRECATED //

// Clean up when done with developing of DL4U_Player and all needed scripts

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

/*
 * DEPRECATED
 * This script controls the behavior of the MENU button on the VIVE Controller 
 */

public class MenuAction : MonoBehaviour
{
    // a reference to the action
    public SteamVR_Action_Boolean openMenu;
    public SteamVR_Input_Sources handType;
    
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
}