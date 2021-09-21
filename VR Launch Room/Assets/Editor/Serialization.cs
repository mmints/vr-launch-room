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
        
        /*
         * @param name: Name of the AssetBundle/Scene
         * @param displayName: A string that should be displayed on the selection GUI
         * @param pathToThumbnailBase64 <- Genau das
         */
        Level level = new Level("rfidscene", "RFID", "Assets/ThumbnailsBase64/tnb64RFIDScene.txt");
        
        string jsonDummyScene = JsonUtility.ToJson(level);

        // TODO: Fill Asset Bundle Path
        
        string jsonDir = "Assets/json";
        if(!Directory.Exists(jsonDir))
        {
            Directory.CreateDirectory(jsonDir);
        }
        File.WriteAllText(jsonDir + "/RFIDScene.json", jsonDummyScene, Encoding.UTF8); // TODO: Level Name
    }
}