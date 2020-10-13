using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;

/*
 * This is a very simplified script. It checks hardcoded game object names.
 * It is currently only used for testing purposes.
 */

public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
    public GameObject placeHolderMain;
    public GameObject placeHolderScene2;
    
    private string main = "Main";
    private string scene2 = "Scene2";
    
    void Awake()
    {
        laserPointer.PointerClick += PointerClick;
    }

    // Use the laserPointer and the Trigger of the controller to enable the transition
    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == main)
        {
            placeHolderMain.SetActive(true);
            Debug.Log("Reset was clicked");
        }
        else if (e.target.name == scene2)
        {
            placeHolderScene2.SetActive(true);
            Debug.Log("Next was clicked");
        }
    }
}