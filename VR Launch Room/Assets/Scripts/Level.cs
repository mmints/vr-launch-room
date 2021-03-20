using System;
using System.IO;
using UnityEngine;

public class Level
{
    public string name;
    public string displayName;
    public string assetBundleURL;
    public string thumbnailBase64;

    // Constructor that deserialize a json to a Level object
    public Level(string json)
    {
        Level level = JsonUtility.FromJson<Level>(json);

        this.name = level.name;
        this.displayName = level.displayName;
        this.assetBundleURL = level.assetBundleURL;
        
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