using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity
{
    public class UnityGameObjectPool : MonoBehaviour
    {
        private static UnityGameObjectPool m_instance;
        private static UnityGameObjectPool instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<UnityGameObjectPool>();
                    if (m_instance == null)
                    {
                        var go = new GameObject("[OBJECT POOL]");
                        m_instance = go.AddComponent<UnityGameObjectPool>();
                    }
                }
                return m_instance;
            }
        }

        private Dictionary<GameObject, List<GameObject>> m_pool = new Dictionary<GameObject, List<GameObject>>();

        public static GameObject InstantiateObject(GameObject go) {
            var keyAvail = false;
            foreach (var dict in instance.m_pool) {
                if (dict.Key == go) {
                    keyAvail = true;
                    foreach (var g in dict.Value) {
                        if (!g.activeInHierarchy) {
                            g.SetActive(true);
                            return g;
                        } 
                    }
                }
            }

            var newGo = Instantiate(go);
            if (keyAvail) {
                instance.m_pool[go].Add(newGo);
            }
            else {
                var newList = new List<GameObject>();
                newList.Add(newGo);
                instance.m_pool.Add(go, newList);
            }
            return newGo;
        }

        public static void DestroyObject(GameObject go) {
            go.SetActive(false);
        }

        public static GameObject[] GetPooledObjects(GameObject prefabs) {
            foreach (var dict in m_instance.m_pool) {
                if (dict.Key == prefabs)
                    return dict.Value.ToArray();
            }
            return null;
        }
    }
}
