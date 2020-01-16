using UnityEditor;
using UnityEngine;
using System.IO;

namespace Editor
{
    public class UploadAssetBundle : EditorWindow
    {
        public string[] options;
        public int index = 0;
        public string myString = "Not Selected by Now";
        string assetBundleDirectory = "Assets/AssetBundles";
        
        void OnEnable()
        {
            options = AssetDatabase.GetAllAssetBundleNames();
        }
        
        // Add menu item named "My Window" to the Window menu
        [MenuItem("Window/Upload Scene to Moodle Server")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(UploadAssetBundle));
        }
        
        void OnGUI()
        {
            GUILayout.Label ("Select the AssetBundle that Should Be Uploaded", EditorStyles.label);
            index = EditorGUILayout.Popup(index, options);
            if (GUILayout.Button("Select"))
            {
                myString = GetPathToScene(index);
            }
            myString = EditorGUILayout.TextField ("Path to Scene", myString);

            if (GUILayout.Button("Build Scene"))
            {
                if(!Directory.Exists(assetBundleDirectory))
                {
                    Directory.CreateDirectory(assetBundleDirectory);
                }
                BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
            }
        }
        
        string GetPathToScene(int selectedAssetBundle)
        {
            string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(options[index]);
            if (paths.Length == 0)
            {
                return "Scene is not linked to selected asset bundle";
            }
            else {
                return paths[0];
            }
        }
    }
}
