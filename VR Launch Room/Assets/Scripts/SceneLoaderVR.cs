using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

/*
 * This script adapts the scene loading functionality from SteamVR_LoadLevel.cs (SteamVR 2.0).
 * 
 * SteamVR_LoadLevel.cs is useful to create smooth transitions between levels.
 * It can be extended with additional information to the user like loading bar
 * or title text etc.
 */

public class SceneLoaderVR : MonoBehaviour
{
    private SteamVR_LoadLevel steamVRLoadLevel;
    void Start()
    {
        EventSystem.current.onLoadLevel += this.LoadLevel;
    }

    private void LoadLevel(string levelName)
    {
        Debug.Log("SceneLoaderVR: Load Level:" + levelName);
        SteamVR_LoadLevel.Begin(levelName, showGrid:true);
    }
}