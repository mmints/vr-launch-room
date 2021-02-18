using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    // Holds the data of the logged in player/user
    public UserData player;
    
    // This is linked to the elements in the UI canvas
    public Text nameText;
    public RawImage screenShot; // Check out, why Image does not work
    
    // Holds all levels that are assigned to the player
    private List<Level> _levelContainers = new List<Level>();
    
    // For iteration trough the level
    private int _currentIdx;
    private int _maxIdx;

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
        LoadLevel();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GetLevelsByPlayerLevelList();
        
        // Initialize Indices
        _currentIdx = 0;
        _maxIdx = _levelContainers.Count() - 1;

        // Present the first Level
        nameText.text = _levelContainers[_currentIdx].name;
        screenShot.texture = _levelContainers[_currentIdx].screenShot;
    }

    void GetLevelsByPlayerLevelList()
    {
        string[] levelNames = player.GetLevels();
        
        // get the path to the levels (here just to the DummyAssetBundles)
        foreach (var levelName in levelNames)
        {
            Level level =
                AssetDatabase.LoadAssetAtPath<Level>("Assets/AssetBundleDummies/" + levelName + "/" + levelName +
                                                     ".asset");
            _levelContainers.Add(level);    
        }
    }

    // Use Buttons or other trigger to scroll through the Levels by using this functions
    public void ShowNextLevel()
    {
        // If the current Index is the max Index then start over from 0
        if (_currentIdx != _maxIdx)
            _currentIdx++;
        else
            _currentIdx = 0;

        nameText.text = _levelContainers[_currentIdx].name;
        screenShot.texture = _levelContainers[_currentIdx].screenShot;
    }

    public void ShowPreviousLevel()
    {
        // If the current Index is 0 then start over from the max Index
        if (_currentIdx != 0)
            _currentIdx--;
        else
            _currentIdx = _maxIdx;

        nameText.text = _levelContainers[_currentIdx].name;
        screenShot.texture = _levelContainers[_currentIdx].screenShot;
    }

    public void LoadLevel()
    {
        Debug.Log("Load Level: " + _levelContainers[_currentIdx].name);
        SceneManager.LoadScene(_levelContainers[_currentIdx].name); // Currently just use the regular scene loader
    }
}