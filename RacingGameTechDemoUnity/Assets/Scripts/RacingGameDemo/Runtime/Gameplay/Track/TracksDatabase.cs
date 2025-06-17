namespace RacingGameDemo.Runtime.Gameplay.Track
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Database;

    [CreateAssetMenu(fileName = TRACKS_DATABASE_ASSET_NAME, menuName = TRACKS_DATABASE_MENU_NAME)]
    public class TracksDatabase : FileDatabase<TrackDetails>
    {
        public const string TRACKS_DATABASE_SCRIPTABLE_OBJECT_PATH = "RacingGameDemo/Tracks/TracksDatabase";

        private const string TRACKS_DATABASE_ASSET_NAME = "TracksDatabase";
        private const string TRACKS_DATABASE_MENU_NAME = "Database/TracksDatabase";

        public override string FileDatabasePathScriptableObjectPath => TRACKS_DATABASE_SCRIPTABLE_OBJECT_PATH;

        protected override string TemplateIdsContainerScriptPath => "Assets/Scripts/RacingGameDemo/Editor/Database/Templates/TemplateTrackIds.txt";
        protected override string IdsContainerClassScriptPath => "Assets/Scripts/RacingGameDemo/Runtime/Gameplay/Track/TrackIds.cs";
        protected override string TemplateIdVariableSlot => "#TrackId#";
        protected override string IdScriptLineStart => "#";
        protected override string IdScriptLineEnd => ",";
    }
}