using System;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Imoet.Unity
{
    public abstract class UnityScriptableObject<T> : ScriptableObject where T : ScriptableObject {
        private static T _instance;
        protected static T _GetInstance(string filePath) {
            if (_instance == null) {
                _instance = Resources.Load<T>(filePath);
                if(_instance != null)
                    (_instance as UnityScriptableObject<T>)._onCreate();
            }   
            return _instance;
        }

        protected virtual void _onCreate() { }
        protected static void _CreateEditorResources(string filePath, Action<T> OnResourceCreated = null) {
#if UNITY_EDITOR
            var path = "Assets/Resources/" + filePath + ".asset";
            var inst = CreateInstance<T>();

            var fInfo = new FileInfo(path);
            var fDir = fInfo.Directory;
            if (!fDir.Exists)
                Directory.CreateDirectory(fDir.FullName);
            
            AssetDatabase.CreateAsset(inst, path);
            AssetDatabase.SaveAssets();
            OnResourceCreated?.Invoke(_GetInstance(path));

            Selection.activeObject = inst;
#else
            //Empty
            return;
#endif
        }
        protected static bool _ValidateEditorResource(string filePath) {
#if UNITY_EDITOR
            var path = "Assets/Resources/" + filePath + ".asset";
            return AssetDatabase.LoadAssetAtPath(path, typeof(T)) != null;
#else
            //Empty
            return false;
#endif
        }
        protected static bool _SelectEditorResources(string filePath) {
#if UNITY_EDITOR
            var path = "Assets/Resources/" + filePath + ".asset";
            var db = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            if(db != null)
                Selection.activeObject = db;
            return db != null;
#else
            //Empty
            return false;
#endif
        }
    }
}
