using System;
using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity.Utility
{
    public static class PlayerPrefsX {
#if USING_NEWTONSOFT_JSON
        private static List<Pair<string, string>> m_values;
#else
        private static List<_PairString> m_values;
#endif

        static PlayerPrefsX() {
#if USING_NEWTONSOFT_JSON
            m_values = new List<Pair<string, string>>();
#else
            m_values = new List<_PairString>();
#endif
        }

        public static T Get<T>(string key) {
            var tType = typeof(T);
            var val = _getValue(key);

            if (!string.IsNullOrEmpty(val)) {
                if (tType.IsPrimitive || tType.Equals(typeof(string)))
                    return (T)Convert.ChangeType(val, typeof(T));
                else
                    return JsonUtil.FromJson<T>(val);
            }
            return default(T);
        }
        public static void Set<T>(string key, T value) {
            var tType = typeof(T);
            string valStr = null;
            if (tType.IsPrimitive || tType.Equals(typeof(string)))
                valStr = value.ToString();
            else
                valStr = JsonUtil.ToJson(value);
            _setValue(key, valStr);
        }
        public static bool HasKey(string key) {
            foreach (var item in m_values)
                if (item.Key == key)
                    return true;
            return false;
        }
        public static bool LoadFromJson(string json) {
            try
            {
                var newVal = JsonUtil.FromJson<_helper>(json);
                if (newVal != null && newVal.values != null) {
#if USING_NEWTONSOFT_JSON
                    m_values = new List<Pair<string, string>>(newVal.values);
#else
                    m_values = new List<_PairString>(newVal.values);
#endif

                    return true;
                }
                Debug.LogError("Failed To Load PlayerPrefsX from Json");
                return false; 
            }
            catch (Exception e) {
                Debug.LogException(e);
                return false;
            }
        }
        public static string SaveToJson() {
            try
            {
                return JsonUtil.ToJson(new _helper() { values = m_values.ToArray() });
            }
            catch (Exception e) {
                Debug.LogException(e);
                return null;
            }
        }

        private static string _getValue(string key) {
            foreach (var item in m_values)
                if (item.Key == key)
                    return item.Value;
            return null;
        }
        private static void _setValue(string key, string value) {
            foreach (var item in m_values)
                if (item.Key == key) {
                    item.Value = value;
                    return;
                }
#if USING_NEWTONSOFT_JSON
            m_values.Add(new Pair<string, string>(key, value));
#else
            m_values.Add(new _PairString(key, value));
#endif
        }

        //STUPID UNITY JsonUtility!!!
        [Serializable]
        private class _helper {
#if USING_NEWTONSOFT_JSON
            public Pair<string,string>[] values;
#else
            public _PairString[] values;
#endif

        }
#if !USING_NEWTONSOFT_JSON
        [Serializable]
        private class _PairString : Pair<string, string> {
            public _PairString() : base(null,null) { }
            public _PairString(string key, string value) : base(key, value) { }
        }
#endif
    }
}
