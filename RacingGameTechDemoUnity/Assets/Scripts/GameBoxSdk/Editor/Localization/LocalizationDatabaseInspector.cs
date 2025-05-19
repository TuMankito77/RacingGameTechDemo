namespace GameBoxSdk.Editor.Localization
{
    using UnityEngine;
    
    using GameBoxSdk.Editor.Database;
    using GameBoxSdk.Runtime.Localization;
    
    using UnityEditor;

    [CustomEditor(typeof(LocalizationDatabase))]
    public class LocalizationDatabaseInspector : GenerateDatabaseButtonInspector<LocalizationDatabase, TextAsset>
    {
    
    }
}

