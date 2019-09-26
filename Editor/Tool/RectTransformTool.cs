using Imoet.Unity.Utility;
using UnityEditor;
using UnityEngine;

namespace Imoet.UnityEditor.Tool
{
    public class RectTransformTool
    {
        [MenuItem("Imoet/UI Tool/Set Anchor... %#&a")]
        static void Execute()
        {
            GameObject[] allG = Selection.gameObjects;
            if (allG == null)
                return;
            foreach (GameObject g in allG)
            {
#if UNITY_2017_1_OR_NEWER
                if (PrefabUtility.GetPrefabAssetType(g) == PrefabAssetType.NotAPrefab) {
#else
                if(PrefabUtility.GetPrefabType(g) == PrefabType.None && g.gameObject.scene.name == null) { 
#endif
                    RectTransform r = g.GetComponent<RectTransform>();
                    if (r != null)
                    {
                        Undo.RecordObject(r, r.GetInstanceID().ToString());
                        r.AutoSetAnchor();
                    }
                    else
                    {
                        Debug.LogWarning("Failed to SetAnchor because selected object doesn't have RectTransform, Ignoring", g);
                    }
                } 
            }
        }
    }
}
