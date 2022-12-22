using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CutsceneEvent))]
public class CutsceneEventDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect line = new Rect(position.xMin, position.yMin, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(line, property.isExpanded, label);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            line = GetNextLineRect(line);
            EditorGUI.PropertyField(line, property.FindPropertyRelative("type"));
            switch ((CutsceneEvent.EventType)property.FindPropertyRelative("type").intValue)
            {
                case CutsceneEvent.EventType.Dialogue:
                    line = GetNextLineRect(line);
                    EditorGUI.ObjectField(line, property.FindPropertyRelative("dialogue"));
                    break;
                case CutsceneEvent.EventType.CharacterMove:
                    line = GetNextLineRect(line);
                    EditorGUI.ObjectField(line, property.FindPropertyRelative("character"));
                    line = GetNextLineRect(line);
                    EditorGUI.ObjectField(line, property.FindPropertyRelative("targetLocation"));
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("velocity"));
                    break;
                case CutsceneEvent.EventType.CharacterJump:
                    line = GetNextLineRect(line);
                    EditorGUI.ObjectField(line, property.FindPropertyRelative("character"));
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("velocity"), new GUIContent("Start Velocity"));
                    break;
                case CutsceneEvent.EventType.Wait:
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("time"));
                    break;
                case CutsceneEvent.EventType.SetActivity:
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("target"), new GUIContent("Object"));
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("setTargetToActive"), new GUIContent("Set to Active"));
                    break;
                case CutsceneEvent.EventType.SetPosition:
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("target"), new GUIContent("Object"));
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("targetLocation"));
                    break;
                case CutsceneEvent.EventType.DisplayText:
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("displayText"));
                    line = GetNextLineRect(line);
                    EditorGUI.PropertyField(line, property.FindPropertyRelative("time"), new GUIContent("Display Time"));
                    break;
            }
            EditorGUI.indentLevel--;

        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded) return GetHeightByLines(1);
        return (CutsceneEvent.EventType)property.FindPropertyRelative("type").intValue switch
        {
            CutsceneEvent.EventType.Dialogue => GetHeightByLines(3),
            CutsceneEvent.EventType.CharacterMove => GetHeightByLines(5),
            CutsceneEvent.EventType.CharacterJump => GetHeightByLines(4),
            CutsceneEvent.EventType.DisplayText => GetHeightByLines(4),
            CutsceneEvent.EventType.Wait => GetHeightByLines(3),
            CutsceneEvent.EventType.SetActivity => GetHeightByLines(4),
            CutsceneEvent.EventType.SetPosition => GetHeightByLines(4),
            _ => GetHeightByLines(2),
        };
    }

    private float GetHeightByLines(int lines)
    {
        return EditorGUIUtility.singleLineHeight * lines + EditorGUIUtility.standardVerticalSpacing * (lines - 1);
    }

    private Rect GetNextLineRect(Rect line)
    {
        return new Rect(line.xMin, line.yMin + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, line.width, EditorGUIUtility.singleLineHeight);
    }
}