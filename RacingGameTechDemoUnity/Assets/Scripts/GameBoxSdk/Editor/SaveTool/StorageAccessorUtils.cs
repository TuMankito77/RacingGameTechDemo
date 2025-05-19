namespace GameBoxSdk.Editor.SaveTool
{
    using UnityEditor;
    using UnityEngine;

    public static class StorageAccessorUtils
    {
        [MenuItem("SaveTool/DeleteAllPlayerPrefs")]
        private static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
