using System.IO;
using UnityEngine;

public class Level
{
    public string name;
    public string displayName;
    public string assetBundleURL;
    public string thumbnailBase64;

    // Constructor that deserialize a json to a Level object
    public Level(string pathToJson)
    {
        string json = File.ReadAllText(pathToJson); // TODO (POC): get this data from moodle connector
        Level level = JsonUtility.FromJson<Level>(json);

        this.name = level.name;
        this.displayName = level.displayName;
        this.assetBundleURL = level.assetBundleURL;
        this.thumbnailBase64 = level.thumbnailBase64;
    }
}