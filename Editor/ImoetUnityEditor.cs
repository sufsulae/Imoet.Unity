using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Imoet.UnityEditor
{
    public abstract class ImoetUnityEditor : Editor {
        private Dictionary<string, SerializedProperty> m_property = new Dictionary<string, SerializedProperty>();
        private Dictionary<SerializedProperty, Dictionary<string, SerializedProperty>> m_subProperty = new Dictionary<SerializedProperty, Dictionary<string, SerializedProperty>>();

        protected SerializedProperty getProperty(string propName) {
            if (m_property.ContainsKey(propName))
                return m_property[propName];
            var prop = serializedObject.FindProperty(propName);
            if (prop != null) {
                m_property.Add(propName, prop);
            }
            else {
                Debug.LogError("Property Not Found! (<b>" + propName + "</b>)");
            }
            return prop;
        }
        protected SerializedProperty getSubProperty(SerializedProperty prop, string propName) {
            if (m_subProperty.ContainsKey(prop) && m_subProperty[prop].ContainsKey(propName))
                return m_subProperty[prop][propName];
            var subProp = prop.FindPropertyRelative(propName);
            if (subProp != null)
            {
                if (m_subProperty.ContainsKey(prop))
                    m_subProperty[prop].Add(propName, subProp);
                else
                {
                    m_subProperty.Add(prop, new Dictionary<string, SerializedProperty>());
                    m_subProperty[prop].Add(propName, subProp);
                }
            }
            else {
                Debug.LogError("Property Not Found! (<b>" + propName + "</b> in property <b>" + prop.name + "</b>)");
            }
            return subProp;
        }
    }
}
