using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is attached to the LevelSelectionTable.
// It manages the downloading, presenting and launching of available Levels. 
public class LevelSelectionTable : MonoBehaviour
{
    // Member Variables
    private User _user;
    private Level _level;
    private AssetBundle _assetBundle;
    public NetworkManager _NetworkManager; // Communication to odl4u.ko-ld.de
    
    // For iteration trough the level
    private int _currentIdx;
    private int _maxIdx;
    
    // GUI Elements
    public RawImage thumbnail;    
    public Text levelTitle;

    void Start()
    {
        // Initialization
        _user = new User("Assets/Moodle/user.json");
        _NetworkManager = new NetworkManager();
        
        _currentIdx = 0;
        _maxIdx = _user.levelNames.Count - 1;
        Debug.Log("INDEX - current: " + _currentIdx + ", max: " + _maxIdx);
    }
    
    // Interface to interact with the menu by the Level Select Controller
    public void LeftButtonClicked()
    {
        Debug.Log("Left Button Clicked!");
        ShowPreviousLevel();
    }
    
    public void RightButtonClicked()
    {
        Debug.Log("Right Button Clicked!");
        ShowNextLevel();
    }
    
    
    // Use Buttons or other trigger to scroll through the Levels by using this functions
    public void ShowNextLevel()
    {
        // If the current Index is the max Index then start over from 0
        if (_currentIdx != _maxIdx)
            _currentIdx++;
        else
            _currentIdx = 0;

        StartCoroutine(DisplayLevel(_user.levelNames[_currentIdx]));
    }

    public void ShowPreviousLevel()
    {
        // If the current Index is 0 then start over from the max Index
        if (_currentIdx != 0)
            _currentIdx--;
        else
            _currentIdx = _maxIdx;

        StartCoroutine(DisplayLevel(_user.levelNames[_currentIdx]));
    }
    
    // Co-Routine to send a GET request to the server and display the title and thumbnail of the selected scene.
    private IEnumerator DisplayLevel(string levelName)
    {
        yield return GetLevel(levelName);
        
        thumbnail.texture = _level.GetImage();
        Debug.Log("Thumbnail is set.");
        
        levelTitle.text = _level.displayName;
        Debug.Log("Title is set.");
    }
    
    // GET request Co-Routine to get the Level Object from the server
    private IEnumerator GetLevel(string levelName)
    {
        Debug.Log("Requesting Level");
        yield return _NetworkManager.GetLevelRequest(levelName);
        Debug.Log("Done!");
        _level = _NetworkManager.GetLevel();
    }
}
