using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace StableEnumUtilities
{
    public static class EditorHelper
    {
        public static List<T> FindAssetsOfType<T>() where T : UnityEngine.Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            var guidsCount = guids.Length;
            var assets = new List<T>(guidsCount);
            for (int i = 0; i < guidsCount; i++)
            {
                var guid = guids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public static void SetTargetObjectOfProperty(SerializedProperty prop, object value)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }

            if (Object.ReferenceEquals(obj, null)) return;

            try
            {
                var element = elements.Last();

                if (element.Contains("["))
                {
                    var tp = obj.GetType();
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    var field = tp.GetField(elementName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    var arr = field.GetValue(obj) as System.Collections.IList;
                    arr[index] = value;
                    //var elementName = element.Substring(0, element.IndexOf("["));
                    //var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    //var arr = DynamicUtil.GetValue(element, elementName) as System.Collections.IList;
                    //if (arr != null) arr[index] = value;
                }
                else
                {
                    var tp = obj.GetType();
                    var field = tp.GetField(element, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                    {
                        field.SetValue(obj, value);
                    }
                    //DynamicUtil.SetValue(obj, element, value);
                }

            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Gets the object the property represents.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                int leftBracketIndex = element.IndexOf("[");
                if (leftBracketIndex >= 0)
                {
                    int rightBracketIndex = element.IndexOf("]", leftBracketIndex);
                    var elementName = element.Substring(0, leftBracketIndex);
                    var index = System.Convert.ToInt32(element.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }

        public static object[] GetTargetObjectsOfProperty(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object[] objs = Array.ConvertAll(prop.serializedObject.targetObjects, (o) => o as object);
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                int leftBracketIndex = element.IndexOf("[");
                if (leftBracketIndex >= 0)
                {
                    int rightBracketIndex = element.IndexOf("]", leftBracketIndex);
                    var elementName = element.Substring(0, leftBracketIndex);
                    var index = System.Convert.ToInt32(element.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1));
                    for (int i = 0, count = objs.Length; i < count; ++i)
                        objs[i] = GetValue_Imp(objs[i], elementName, index);
                }
                else
                {
                    for (int i = 0, count = objs.Length; i < count; ++i)
                        objs[i] = GetValue_Imp(objs[i], element);
                }
            }
            return objs;
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }
    }
}
