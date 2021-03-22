using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Serialization
{
    [MenuItem("DigiLab4U/Serialize")]
    public static void Serialize()
    {
        // Create Levels
        // TODO: Create GUI to Serialize Selected Level
        // -> Currently: Exchange the params in this call
        Level level = new Level("name", "displayName", "path");
        
        string jsonDummyScene = JsonUtility.ToJson(level);

        string jsonDir = "Assets/json";
        if(!Directory.Exists(jsonDir))
        {
            Directory.CreateDirectory(jsonDir);
        }
        File.WriteAllText(jsonDir + "/DummyScene.json", jsonDummyScene, Encoding.UTF8); // TODO: Level Name
    }
}