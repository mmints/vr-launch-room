using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Level
{
    public string name;
    public string displayName;
    public string assetBundleURL;
    public string thumbnailBase64;

    // Constructor, used to serialize Level
    public Level(string name, string displayName, string pathToThumbnailBase64)
    {
        this.name = name;
        this.displayName = displayName;
        this.assetBundleURL = ""; // Redundant
        this.thumbnailBase64 =  File.ReadAllText(pathToThumbnailBase64);
    }
    
    // Constructor that deserialize a json to a Level object
    public Level(string json)
    {
        Level level = JsonUtility.FromJson<Level>(json);

        this.name = level.name;
        this.displayName = level.displayName;
        this.assetBundleURL = level.assetBundleURL; // TODO: Redundant -> Remove Attribute?
                                                    // Attention:
                                                    // Currently some Level contain this field
                                                    // hence, there will be an error in deserialization
                                                    
        // Remove redundant data information from base64 encodeing before the ',' -> data:image/png;base64
        string[] subs = level.thumbnailBase64.Split(',');
        this.thumbnailBase64 = subs[1];
    }
    
    // Returns the encoded thumbnail image as a Texture2D.
    // This is used to display the image on RawImage Object on a GUI Canvas.
    public Texture2D GetImage()
    {
        byte[] bytes = Convert.FromBase64String(thumbnailBase64);
        Texture2D tex = new Texture2D(300,200);
        Debug.Log("Decoded Image loaded to Texture2D.");
        
        tex.LoadImage(bytes);
        return tex;
    }
}