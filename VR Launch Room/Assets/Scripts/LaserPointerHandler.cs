using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

/*
 * Triggers the internal SteamVR_LaserPointer to activate and deactivate.
 *
 * ATTENTION:
 * The initial SteamVR_LaserPointer.cs script is buggy!
 * There is a public property active (bool) in the class, but it is never accessed in the code.
 * The fix can be found here:
 * https://gitlab.uni-koblenz.de/open-digital-lab-4you/steamvr-internal-fixes/-/blob/master/SteamVR_LaserPointer.cs
 */

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
