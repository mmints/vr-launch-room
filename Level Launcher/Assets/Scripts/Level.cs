using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public new string name;
    
    public int version; // The Version number is needed for Caching
    public string pathToScene; // ?rename to pathToAssetBundle?

    public Texture screenShot;
}