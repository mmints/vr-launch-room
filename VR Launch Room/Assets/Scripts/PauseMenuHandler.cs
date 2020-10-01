using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
 * This script handles the appearance of the pause menu.
 */

public class PauseMenuHandler : MonoBehaviour
{
    public SteamVR_Action_Boolean openMenu;
    public SteamVR_Input_Sources handType;
    
    public Transform cameraTransform;
    public GameObject pauseMenuUI;
    
    public static bool gameIsPaused = false; // Indicates if the menu is currently open or not

    void Start()
    {
        openMenu.AddOnStateDownListener(OnMenuButton, handType);
    }
    
    void OnMenuButton(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Menu Button Clicked!");

        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    
    void Update()
    {
        // Use keyboard input for quick debugging
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
    }

    // Opens a pause menu at a fixed position in the world relatively to the head/camera position
    // Works just like the ingame menu in SteamVR Home
    void Pause()
    {
        pauseMenuUI.transform.rotation = cameraTransform.rotation;
        pauseMenuUI.transform.position = cameraTransform.position + (cameraTransform.forward * 3f);
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }
}