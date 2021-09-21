using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User
{
    public string name; // optional
    public List<string> levelNames; // simple version

    public List<Level> levels; // advanced version

    // Constructor that deserialize a json to a User object
    public User(string pathToJson)
    {
        string json = File.ReadAllText(pathToJson); // TODO (POC): get this data from moodle connector
        User user = JsonUtility.FromJson<User>(json);

        this.name = user.name;
        this.levelNames = user.levelNames;
        this.levels = user.levels;
    }
}