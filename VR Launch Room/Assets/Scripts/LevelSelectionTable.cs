using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;

// This script is attached to the LevelSelectionTable.
// It manages the downloading, presenting and launching of available Levels. 
public class LevelSelectionTable : MonoBehaviour
{
    // Member Variables
    private User _user;
    private List<Level> _levels; // Use this for caching the levels
    private AssetBundle _assetBundle;
    private NetworkManager _NetworkManager; // Communication to odl4u.ko-ld.de
    
    // For iteration trough the level
    private int _currentIdx;
    private int _maxIdx;
    
    // GUI Elements
    public RawImage thumbnail;    
    public Text levelTitle;

    void Start()
    {
        // Initialization
        _user = new User("Assets/Moodle/user.json"); // TODO: This only works in the Editor!
        // For Build: 
        // Add a json file with the user credentials right in the directory of the .exe
        // Adjust the path to "user.json"
        
        _NetworkManager = new NetworkManager();

        // Init the Idx for browsing through the available levels
        _currentIdx = 0;
        _maxIdx = _user.levelNames.Count - 1;
        Debug.Log("INDEX - current: " + _currentIdx + ", max: " + _maxIdx);
        
        Debug.Log("Filling up levels.");
        _levels = new List<Level>();
        StartCoroutine(GetLevels()); // fill _levels List in Co-Routine
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
    
    public void LoadButtonClicked()
    {
        Debug.Log("Load Button Clicked!");
        StartCoroutine(LoadLevel()); // Wait until finished and then load the selected level
    }
    
    // Use Buttons or other trigger to brows through the Levels by using this functions
    public void ShowNextLevel()
    {
        // If the current Index is the max Index then start over from 0
        if (_currentIdx != _maxIdx)
            _currentIdx++;
        else
            _currentIdx = 0;

        thumbnail.texture = _levels[_currentIdx].GetImage();
        levelTitle.text = _levels[_currentIdx].displayName;
    }

    public void ShowPreviousLevel()
    {
        // If the current Index is 0 then start over from the max Index
        if (_currentIdx != 0)
            _currentIdx--;
        else
            _currentIdx = _maxIdx;

        thumbnail.texture = _levels[_currentIdx].GetImage();
        levelTitle.text = _levels[_currentIdx].displayName;
    }
    
    private IEnumerator LoadLevel() // Will be called by ButtonDown function of the top game object
    {
        levelTitle.text = "Loading: " + _levels[_currentIdx].name;
        yield return GetAssetBundle(); // Download or load from cache
        
        // Get the name of the first Scene like organized in the Building Setting from the
        // AssetBundle's origin Project. Hence, it is the starting Scene.
        string startScenePath = _assetBundle.GetAllScenePaths()[0];
        Debug.Log("Start Scene: " + startScenePath);
        
        SceneManager.LoadScene(startScenePath);
        var scene = SceneManager.GetSceneByName(startScenePath);
        Debug.Log("Load scene: " + scene.name);

        _assetBundle.Unload(true); // Unloads an AssetBundle freeing its data.
        // In either case you won't be able to load any more objects from this bundle unless it is reloaded.
    }
    
    // Send GET requests to download the selected level
    private IEnumerator GetAssetBundle()
    {
        Debug.Log("Requesting Asset Bundle");
        yield return _NetworkManager.GetAssetBundleRequest(_levels[_currentIdx].name);
        Debug.Log("Done!");
        _assetBundle = _NetworkManager.GetAssetBundle();
    }
    
    // Send GET requests for all levels that are available for the user
    private IEnumerator GetLevels()
    {
        levelTitle.text = "Connecting..."; // Display on the GUI that the request is going on
        
        Debug.Log("Get all Levels.");
        foreach (var levelName in _user.levelNames)
        {
            yield return _NetworkManager.GetLevelRequest(levelName);
            _levels.Add(_NetworkManager.GetLevel());   
        }
        Debug.Log("Done!");
        Debug.Log("Level Count: " + _levels.Count);
        
        // When done, display the first level on the screen
        thumbnail.texture = _levels[_currentIdx].GetImage();
        levelTitle.text = _levels[_currentIdx].displayName;
    }
}