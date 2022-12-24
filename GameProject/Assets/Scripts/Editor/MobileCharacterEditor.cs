using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MobileCharacter))]
public class MobileCharacterEditor : Editor
{
    private SerializedProperty groundLayer;
    private SerializedProperty animated;
    private SerializedProperty jumpAnimationName;

    private void OnEnable()
    {
        groundLayer = serializedObject.FindProperty("groundLayer");
        animated = serializedObject.FindProperty("animated");
        jumpAnimationName = serializedObject.FindProperty("jumpAnimationName");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(groundLayer);
        EditorGUILayout.PropertyField(animated);
        if (animated.boolValue)
        {
            EditorGUILayout.PropertyField(jumpAnimationName, new GUIContent("Jump Animation"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}