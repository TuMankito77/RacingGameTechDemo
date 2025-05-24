namespace GameBoxSdk.Runtime.Sound
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Database;

    [CreateAssetMenu(fileName = CLIPS_DATABASE_ASSET_NAME, menuName = "Database/ClipsIdDatabase")]
    public class ClipsDatabase : FileDatabase<AudioClip>
    {
        public const string CLIPS_DATABASE_SCRIPTABLE_OBJECT_PATH = "Sound/ClipsDatabase";
        
        private const string CLIPS_DATABASE_ASSET_NAME = "ClipsDatabase";

        public override string FileDatabasePathScriptableObjectPath => CLIPS_DATABASE_SCRIPTABLE_OBJECT_PATH;
        
        protected override string TemplateIdsContainerScriptPath => "Assets/Scripts/GameBoxSdk/Editor/Database/Templates/TemplateClipIds.txt";
        protected override string IdsContainerClassScriptPath => "Assets/Scripts/GameBoxSdk/Runtime/Sound/ClipIds.cs";
        protected override string TemplateIdVariableSlot => "#ClipId#";
        protected override string IdScriptLineStart => "#";
        protected override string IdScriptLineEnd => ",";
    }
}
