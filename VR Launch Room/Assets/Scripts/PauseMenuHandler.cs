using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles the appearance of the pause menu.
 */

public class PauseMenuHandler : MonoBehaviour
{
    public Transform pauseMenuPosition; // Place pause menu at this position
    public GameObject pauseMenu;
    
    private bool menuIsOpen = false; // Indicates if the menu is currently open or not
    
    void Update()
    {
        // Use keyboard input for quick debugging
        if (Input.GetKeyUp(KeyCode.Space) && !menuIsOpen)
        {
            Instantiate(pauseMenu, pauseMenuPosition.position, pauseMenuPosition.rotation);
            menuIsOpen = true;
        }
    }
}