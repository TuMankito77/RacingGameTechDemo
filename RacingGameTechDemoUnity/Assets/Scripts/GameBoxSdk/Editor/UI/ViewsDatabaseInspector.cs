namespace GameBoxSdk.Editor.UI
{
    using UnityEditor;

    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Editor.Database;

    [CustomEditor(typeof(ViewsDatabase))]
    public class ViewsDatabaseInspector : GenerateDatabaseButtonInspector<ViewsDatabase, BaseView>
    {
        
    }
}

