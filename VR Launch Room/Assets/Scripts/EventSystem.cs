using System;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current; // Current Event

    void Awake()
    {
        current = this;
    }

    public event Action<string> loadLevel;

    public void LoadLevel(string levelName)
    {
        if (loadLevel != null)
        {
            loadLevel(levelName);
            Debug.Log("EventSystem: Button was Triggered!");
        }
    }
}
