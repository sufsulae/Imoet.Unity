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
            var inst = CreateInstance<T>();

            var fInfo = new FileInfo(filePath);
            var fDir = fInfo.Directory;
            if (!fDir.Exists)
                Directory.CreateDirectory(fDir.FullName);
            
            AssetDatabase.CreateAsset(inst, filePath);
            AssetDatabase.SaveAssets();
            OnResourceCreated?.Invoke(_GetInstance(filePath));

            Selection.activeObject = inst;
#else
            //Empty
            return;
#endif
        }
        protected static bool _ValidateEditorResource(string filePath) {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath(filePath, typeof(T)) != null;
#else
            //Empty
            return false;
#endif
        }

        protected static bool _SelectEditorResources(string filePath) {
#if UNITY_EDITOR
            var db = AssetDatabase.LoadAssetAtPath(filePath, typeof(T));
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
