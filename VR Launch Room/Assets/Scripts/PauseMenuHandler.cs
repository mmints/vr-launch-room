using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles the appearance of the pause menu.
 */

public class PauseMenuHandler : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject pauseMenuUI;
    
    public static bool gameIsPaused = false; // Indicates if the menu is currently open or not
    
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
    void Pause()
    {
        pauseMenuUI.transform.rotation = cameraTransform.rotation;
        pauseMenuUI.transform.position = cameraTransform.position + (cameraTransform.forward * 3f);
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }
}