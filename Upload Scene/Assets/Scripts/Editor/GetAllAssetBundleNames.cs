using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GetAssetBundleNames : UnityEditor.Editor
    {
        [MenuItem("Build/Get Asset Bundle names")]
        static void GetNames()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (string name in names)
                Debug.Log("Asset Bundle: " + name);
        }

        [MenuItem("Build/Get Path to Assets from asset bundle")]
        static void GetPath()
        {
            var names = AssetDatabase.GetAssetPathsFromAssetBundle("dummy");
            Debug.Log("Path to asset bundle 'dummy': " + names[0]);
        }
    }
}