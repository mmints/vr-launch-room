using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class UploadAssetBundle : EditorWindow
    {
        // Add menu item named "My Window" to the Window menu
        [MenuItem("Window/Upload Scene to Moodle Server")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(UploadAssetBundle));
        }

    }
}
