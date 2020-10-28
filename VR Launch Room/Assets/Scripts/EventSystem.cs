using System;
using UnityEngine;

/*
 * This script handles the internal event system of this application.
 *
 * Current Actions:
 * OnLoadLevel      - Triggers, if a level loading request appears
 * OnEnterTableArea - Triggers, if something (player) enters the area around the controller table
 * OnExitTableArea - Triggers, if something (player) exits the area around the controller table
 */

public class EventSystem : MonoBehaviour
{
    public static EventSystem current; // Current Event

    void Awake()
    {
        current = this;
    }

    public event Action<string> onLoadLevel;
    public event Action onEnterTableArea;
    public event Action onExitTableArea;
    
    public void OnLoadLevel(string levelName)
    {
        if (onLoadLevel != null)
        {
            onLoadLevel(levelName);
            Debug.Log("EventSystem: Button was Triggered!");
        }
    }
    
    public void OnEnterTableArea()
    {
        if (onEnterTableArea != null)
        {
            onEnterTableArea();
            Debug.Log("EventSystem: Player ENTERS the table area!");
        }
    }

    public void OnExitTableArea()
    {
        if (onExitTableArea != null)
        {
            onExitTableArea();
            Debug.Log("EventSystem: Player EXITS the table area!");
        }
    }
}
