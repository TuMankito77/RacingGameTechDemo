namespace GameBoxSdk.Runtime.UI.Views
{
    using GameBoxSdk.Runtime.Database;
    using UnityEngine;

    [CreateAssetMenu(fileName = VIEWS_DATABASE_ASSET_NAME, menuName = "Database/ViewsDatabase")]
    public class ViewsDatabase : FileDatabase<BaseView>
    {
        public const string VIEWS_DATABASE_SCRIPTABLE_OBJECT_PATH = "UI/ViewsDatabase";

        private const string VIEWS_DATABASE_ASSET_NAME = "ViewsDatabase";
        
        [SerializeField]
        private LayerMask viewsLayerMask = default(LayerMask);

        public LayerMask ViewsLayerMask => viewsLayerMask;
        public override string FileDatabasePathScriptableObjectPath => VIEWS_DATABASE_SCRIPTABLE_OBJECT_PATH;

        protected override string TemplateIdsContainerScriptPath => "Assets/Scripts/Editor/Database/Templates/TemplateViewIds.txt";
        protected override string IdsContainerClassScriptPath => "Assets/Scripts/Runtime/UI/Views/ViewIds.cs";
        protected override string TemplateIdVariableSlot => "#ViewId#";
        protected override string IdScriptLineStart => "#";
        protected override string IdScriptLineEnd => ";";
    }
}

