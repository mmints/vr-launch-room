using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

// Use this script to trigger options and setting in Steam VR
public class SteamVRSettings : MonoBehaviour
{
    public bool enableTeleportHint = false;
    
    void Start()
    {
        if (!enableTeleportHint)
            Teleport.instance.CancelTeleportHint (); // Disable teleportation hint
    }
}
