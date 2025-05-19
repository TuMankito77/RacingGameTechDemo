namespace GameBoxSdk.Editor.Sound
{
    using UnityEditor;
    
    using GameBoxSdk.Editor.Database;
    using GameBoxSdk.Runtime.Sound;
    using UnityEngine;

    [CustomEditor(typeof(ClipsDatabase))]
    public class ClipsDatabaseInspector : GenerateDatabaseButtonInspector<ClipsDatabase, AudioClip>
    {
        
    }
}

