using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

/*
 * This script handles the appearance of the pause menu.
 */

public class PauseMenuHandler : MonoBehaviour
{
    // Spawning position of the UI Canvas
    private float tilt = 5f;
    private float height = -0.5f;
    private float distance = 1.25f;
    
    // Button Names
    private string _mainScene = "Main"; // The main controlling scene (Launch Room)
    private string _resume = "Resume"; // Close the Pause Menu on Button click
    
    // Controller options
    public SteamVR_Action_Boolean openMenu;   // SteamVR Controller Action
    public SteamVR_Input_Sources handType;    // The Hand (Controller) that should trigger the Action
    public SteamVR_LaserPointer laserPointer; // The Hand (Controller) that has the SteamVR_LaserPointer.cs Script
    
    public Transform cameraTransform;  // World Space Position of the Players Head to Calculate the View Direction
    public GameObject pauseMenuUI;     // Game object, that contains a canvas with the GUI elements

    // TODO: Turn gameIsPaused to public to use it outside as trigger for pause functions
    private bool gameIsPaused = false; // Indicates if the menu is currently open or not
    
    void Awake()
    {
        openMenu.AddOnStateDownListener(OnMenuButton, handType);
        laserPointer.PointerClick += PointerClick; // Add PointerClick to Laser Pointer Action
    }
    
    // Open Menu on MenuButton click on the Controller
    void OnMenuButton(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Menu Button Clicked!");

        if (gameIsPaused)
            Resume();
        else
            Pause();
    }
    
    // Trigger Function -> Called when the laser pointer targets a valid object
    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == _mainScene)
        {
            Debug.Log("Main Scene was clicked");
            LoadLevel(_mainScene);
            // Make sure that the menu is closed when getting into a new scene
            Resume();
        }
        if (e.target.name == _resume)
        {
            Debug.Log("Resume was clicked");
            // Make sure that the menu is closed when getting into a new scene
            Resume();
        }
    }
    
    // Opens a pause menu at a fixed position in the world relatively to the head/camera position
    // Works just like the ingame menu in SteamVR Home
    void Pause()
    {
        pauseMenuUI.transform.localEulerAngles = new Vector3(tilt, cameraTransform.localEulerAngles.y, 0f);

        // Move the menu to the position of the player and transform it in front of it
        var position = cameraTransform.position + (cameraTransform.forward * distance);
        position = new Vector3(position.x, cameraTransform.transform.position.y + height, position.z);
        pauseMenuUI.transform.position = position;

        laserPointer.active = true;
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }
    
    // Close the Pause Menu and get back to the game.
    void Resume()
    {
        laserPointer.active = false;
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
    }
    
    // Transfers the player to the selected scene
    private void LoadLevel(string levelName)
    {
        Debug.Log("SceneLoaderVR: Load Level:" + levelName);
        SteamVR_LoadLevel.Begin(levelName, showGrid:true);
    }
}