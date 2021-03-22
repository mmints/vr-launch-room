using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
 * Transfers the player from the initial scene to the main scene.
 */

public class     InitialSceneTransfer : MonoBehaviour
{
    public float activationTime;
    private SteamVR_LoadLevel loadLevel;
    void Start()
    {
        loadLevel = GetComponent<SteamVR_LoadLevel>();
    }

    void Update()
    {
        activationTime -= Time.deltaTime;
        if (activationTime < 0 && !loadLevel.enabled)
        {
            Debug.Log("Time is up, load Main Scene!");
            loadLevel.enabled = true;
        }
    }
}
