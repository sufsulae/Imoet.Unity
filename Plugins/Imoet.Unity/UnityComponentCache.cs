using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity {
    public static class UnityComponentCache
    {
        private static Dictionary<GameObject, List<Component>> m_cache;
        static UnityComponentCache()
        {
            m_cache = new Dictionary<GameObject, List<Component>>();
        }

        public static T GetComponentCached<T>(this GameObject go) where T : Component
        {
            if (m_cache.ContainsKey(go))
            {
                var comps = m_cache[go];
                var t = typeof(T);
                foreach (var comp in comps)
                {
                    if (t == comp.GetType())
                        return (T)comp;
                }
                var newComp = go.GetComponent<T>();
                comps.Add(newComp);
                return newComp;
            }
            else
            {
                var newComps = new List<Component>();
                var newComp = go.GetComponent<T>();
                newComps.Add(newComp);
                m_cache.Add(go, newComps);
                return newComp;
            }
        }

        public static T GetComponentCached<T>(this Component obj) where T : Component
        {
            return GetComponentCached<T>(obj.gameObject);
        }

        public static void ClearCache()
        {
            m_cache = new Dictionary<GameObject, List<Component>>();
        }
    }
}