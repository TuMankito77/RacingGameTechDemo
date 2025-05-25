namespace GameBoxSdk.Runtime.UI.Views
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Database;

    [CreateAssetMenu(fileName = VIEWS_DATABASE_ASSET_NAME, menuName = VIEWS_DATABASE_MENU_NAME)]
    public class ViewsDatabase : FileDatabase<BaseView>
    {
        public const string VIEWS_DATABASE_SCRIPTABLE_OBJECT_PATH = "GameBox/UI/ViewsDatabase";

        private const string VIEWS_DATABASE_ASSET_NAME = "ViewsDatabase";
        private const string VIEWS_DATABASE_MENU_NAME = "Database/ViewsDatabase";

        [SerializeField]
        private LayerMask viewsLayerMask = default(LayerMask);

        public LayerMask ViewsLayerMask => viewsLayerMask;
        public override string FileDatabasePathScriptableObjectPath => VIEWS_DATABASE_SCRIPTABLE_OBJECT_PATH;

        protected override string TemplateIdsContainerScriptPath => "Assets/Scripts/GameBoxSdk/Editor/Database/Templates/TemplateViewIds.txt";
        protected override string IdsContainerClassScriptPath => "Assets/Scripts/GameBoxSdk/Runtime/UI/Views/ViewIds.cs";
        protected override string TemplateIdVariableSlot => "#ViewId#";
        protected override string IdScriptLineStart => "#";
        protected override string IdScriptLineEnd => ",";
    }
}

