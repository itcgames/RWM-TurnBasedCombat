using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FirstStrikeChance)), CanEditMultipleObjects]
public class FirstStrikeEditor : Editor
{
    public SerializedProperty
          type_prop,
          onAdvantage_prop,
          attribute_prop;

    private void OnEnable()
    {
        type_prop = serializedObject.FindProperty("m_type");
        onAdvantage_prop = serializedObject.FindProperty("m_onAdvantage");
        attribute_prop = serializedObject.FindProperty("m_attribute");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(type_prop);

        FirstStrikeChance.CheckType type = (FirstStrikeChance.CheckType)type_prop.enumValueIndex;

        switch (type)
        {
            case FirstStrikeChance.CheckType.BooleanExpressions:
                EditorGUILayout.PropertyField(onAdvantage_prop, new GUIContent("onAdvantage"));
                break;
            case FirstStrikeChance.CheckType.Random:
                break;
            case FirstStrikeChance.CheckType.RandomInfluencedByAnATTR:
                EditorGUILayout.PropertyField(attribute_prop, new GUIContent("attribute"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
