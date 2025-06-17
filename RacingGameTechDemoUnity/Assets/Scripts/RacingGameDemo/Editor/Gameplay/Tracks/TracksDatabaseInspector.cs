namespace RacingGameDemo.Editor.Gameplay.Tracks
{
    using UnityEditor;

    using GameBoxSdk.Editor.Database;

    using RacingGameDemo.Runtime.Gameplay.Track;

    [CustomEditor(typeof(TracksDatabase))]
    public class TracksDatabaseInspector : GenerateDatabaseButtonInspector<TracksDatabase, TrackDetails>
    {
    
    }
}
