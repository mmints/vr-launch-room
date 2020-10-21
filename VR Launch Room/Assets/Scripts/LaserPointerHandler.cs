using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

public class LaserPointerHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer; // The Hand (Controller) that has the SteamVR_LaserPointer.cs Script

    void Start()
    {
        EventSystem.current.onEnterTableArea += this.ActivateLaserPointer;
        EventSystem.current.onExitTableArea += this.DeactivateLaserPointer;
    }
    private void ActivateLaserPointer()
    {
        Debug.Log("LaserPointerHandler: Activate Laser Pointer");
        laserPointer.active = true;
    }
    
    private void DeactivateLaserPointer()
    {
        Debug.Log("LaserPointerHandler: Deactivate Laser Pointer");
        laserPointer.active = false;
    }
}
