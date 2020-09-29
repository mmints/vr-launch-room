<<<<<<< HEAD
﻿using System.Collections;
=======
﻿/* SceneHandler.cs*/
using System.Collections;
>>>>>>> bdc65648a13258d04d80c8a2391241ff589364df
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

<<<<<<< HEAD
/*
 * This is a very simplified script. It checks hardcoded game object names.
 * It is currently only used for testing purposes.
 */

public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
    public GameObject placeHolder;
=======
public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
>>>>>>> bdc65648a13258d04d80c8a2391241ff589364df

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was clicked");
        } else if (e.target.name == "Button")
        {
<<<<<<< HEAD
            placeHolder.SetActive(true);
=======
>>>>>>> bdc65648a13258d04d80c8a2391241ff589364df
            Debug.Log("Button was clicked");
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was entered");
        }
        else if (e.target.name == "Button")
        {
            Debug.Log("Button was entered");
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was exited");
        }
        else if (e.target.name == "Button")
        {
            Debug.Log("Button was exited");
        }
    }
}