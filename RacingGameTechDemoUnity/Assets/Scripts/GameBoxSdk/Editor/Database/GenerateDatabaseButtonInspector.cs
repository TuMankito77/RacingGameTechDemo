namespace GameBoxSdk.Editor.Database
{
    using UnityEngine;
    using UnityEditor;

    using GameBoxSdk.Runtime.Database;

    public class GenerateDatabaseButtonInspector<T1, T2> : Editor where T1 : FileDatabase<T2> where T2 : class
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            T1 fileIdsDatabase = target as T1;
            
            if (GUILayout.Button($"Regenerate {fileIdsDatabase.GetType().Name} ID class file"))
            {
                fileIdsDatabase.GenerateIdsContainerClassFile();
            }
        }
    }
}

