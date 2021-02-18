using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This object holds all information coming from Moodle etc.
// After logging into the VR-LMS this data is transferred.

// This is currently just handled as a dummy

[CreateAssetMenu(fileName = "NewUserDataContainer", menuName = "UserDataContainer")]
public class  UserData : ScriptableObject
{
    // Just some dummy stuff to get the idea what this container class is
    [Header("Dummy informations")]
    public new string name;
    public string eMail;

    // Holds an array of level names that are saved in a DB.
    private string[] levels = new []{"red", "blue", "green"};

    // returns the list of the levels assigned to the logged in user
    public string[] GetLevels()
    {
        return levels;
    }
}