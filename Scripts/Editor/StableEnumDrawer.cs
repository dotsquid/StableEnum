using System;
using UnityEditor;
using UnityEngine;
using StableEnumUtilities;

[CustomPropertyDrawer(typeof(BaseStableEnum), true)]
public class StableEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var proxyProp = property.FindPropertyRelative(BaseStableEnum.kProxyPropName);
        var indexProp = property.FindPropertyRelative(BaseStableEnum.kIndexPropName);
        var valueProp = property.FindPropertyRelative(BaseStableEnum.kValuePropName);
        var propObject = EditorHelper.GetTargetObjectOfProperty(property);

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

        var enumValue = (Enum)((BaseStableEnum)propObject).valueObject;
        enumValue = EditorGUI.EnumPopup(position, label, enumValue);

        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            int intValue = (int)Convert.ChangeType(enumValue, enumValue.GetType());
            valueProp.intValue = intValue;
            indexProp.intValue = intValue;
            proxyProp.stringValue = enumValue.ToString();
        }
    }
}
