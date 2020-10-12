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
    private float tilt = 5f;
    private float height = -0.5f;
    private float distance = 1.25f;
    
    public SteamVR_Action_Boolean openMenu;
    public SteamVR_Input_Sources handType;
    
    public Transform cameraTransform;
    public GameObject pauseMenuUI; // Game object, that contains a canvas with the GUI elements

    public GameObject laserPointer; // Game object that contains the component: SteamVR Laser Pointer (Script)
    
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
        laserPointer.SetActive(false);
        gameIsPaused = false;
    }

    // Opens a pause menu at a fixed position in the world relatively to the head/camera position
    // Works just like the ingame menu in SteamVR Home
    void Pause()
    {
        pauseMenuUI.transform.localEulerAngles = new Vector3(tilt, cameraTransform.localEulerAngles.y, 0f);

        // Move the menu to the position of the player and transform it in front of it
        var position = pauseMenuUI.transform.position;
        position = cameraTransform.position + (cameraTransform.forward * distance);
        position = new Vector3(position.x, cameraTransform.transform.position.y + height, position.z);
        pauseMenuUI.transform.position = position;
        
        pauseMenuUI.SetActive(true);
        laserPointer.SetActive(true);
        gameIsPaused = true;
    }
}