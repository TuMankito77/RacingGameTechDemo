namespace GameBoxSdk.Runtime.Localization
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Database;

    [CreateAssetMenu(fileName = LOCALIZATION_DATABASE_ASSET_NAME, menuName = "Database/LocalizationDatabase")]
    public class LocalizationDatabase : FileDatabase<TextAsset>
    {
        public const string LOCALIZATION_DATABASE_SCRIPTABLE_OBJECT_PATH = "GameBox/Localization/LocalizationDatabase";

        private const string LOCALIZATION_DATABASE_ASSET_NAME = "LocalizationDatabase";

        public override string FileDatabasePathScriptableObjectPath => LOCALIZATION_DATABASE_SCRIPTABLE_OBJECT_PATH;

        protected override string TemplateIdsContainerScriptPath => "Assets/Scripts/GameBoxSdk/Editor/Database/Templates/TemplateLanguageIds.txt";
        protected override string IdsContainerClassScriptPath => "Assets/Scripts/GameBoxSdk/Runtime/Localization/LanguageIds.cs";
        protected override string TemplateIdVariableSlot => "#LanguageId#";
        protected override string IdScriptLineStart => "#";
        protected override string IdScriptLineEnd => ",";
    }
}

