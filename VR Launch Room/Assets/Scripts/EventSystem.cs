using System;
using UnityEngine;

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
    
    public void LoadLevel(string levelName)
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
