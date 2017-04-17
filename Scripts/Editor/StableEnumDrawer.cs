using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BaseStableEnum), true)]
public class StableEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var proxyProp = property.FindPropertyRelative(BaseStableEnum.kProxyPropName);
        var valueProp = property.FindPropertyRelative(BaseStableEnum.kValuePropName);
        Enum enumValue = (Enum)((BaseStableEnum)fieldInfo.GetValue(property.serializedObject.targetObject)).valueObject;

        enumValue = EditorGUI.EnumPopup(position, label, enumValue);
        valueProp.intValue = (int)Convert.ChangeType(enumValue, enumValue.GetType());
        proxyProp.stringValue = enumValue.ToString();
    }
}
