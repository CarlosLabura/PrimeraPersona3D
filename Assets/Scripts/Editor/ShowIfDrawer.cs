using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(
        SerializedProperty property,
        GUIContent label)
    {
        if (!ShouldShow(property))
            return 0;

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override void OnGUI(
        Rect position,
        SerializedProperty property,
        GUIContent label)
    {
        if (ShouldShow(property))
        {
            EditorGUI.PropertyField(
                position,
                property,
                label,
                true
            );
        }
    }
    private bool ShouldShow(SerializedProperty property)
    {
        ShowIfAttribute attribute = (ShowIfAttribute)this.attribute;

        SerializedProperty condition;

        if (attribute.componentType == null)
        {
            condition = property.serializedObject.FindProperty(
                attribute.variableName
            );
        }
        else
        {
            Component component =
                property.serializedObject.targetObject as Component;

            if (component == null)
                return false;

            Component targetComponent =
                component.GetComponent(attribute.componentType);

            if (targetComponent == null)
                return false;

            SerializedObject targetObject =
                new SerializedObject(targetComponent);

            condition = targetObject.FindProperty(
                attribute.variableName
            );
        }
        bool value = condition != null && condition.boolValue;
        return attribute.inverse ? !value : value;
    }
}