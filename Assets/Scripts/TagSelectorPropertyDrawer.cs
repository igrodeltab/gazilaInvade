using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            // Проверяем, что это строка и отображаем как тег
            EditorGUI.BeginProperty(position, label, property);

            // Показываем выпадающий список с тегами
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);

            EditorGUI.EndProperty();
        }
        else
        {
            // Показываем сообщение об ошибке, если тип не строковый
            EditorGUI.PropertyField(position, property, label);
            Debug.LogWarning("TagSelectorAttribute can only be used on string properties.");
        }
    }
}