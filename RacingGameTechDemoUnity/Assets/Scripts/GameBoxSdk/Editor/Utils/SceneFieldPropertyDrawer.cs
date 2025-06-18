namespace GameBoxSdk.Editor.Utils
{
    using UnityEditor;
    
    using UnityEngine;

    using GameBoxSdk.Runtime.Utils;

    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty scene = property.FindPropertyRelative("scene");
            SerializedProperty sceneName = property.FindPropertyRelative("sceneName");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if(scene != null)
            {
                scene.objectReferenceValue = EditorGUI.ObjectField(position, scene.objectReferenceValue, typeof(SceneAsset), false);

                if(scene.objectReferenceValue != null)
                {
                    sceneName.stringValue = (scene.objectReferenceValue as SceneAsset).name;
                }
            }

            EditorGUI.EndProperty();
        }
    }
}

