using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace Imoet.Unity {
    public class UnityObjectsCache : MonoBehaviour
    {
        private static UnityObjectsCache _instance
        {
            get
            {
                if (m_instance == null)
                {
                    var newGO = new GameObject("_UnityObjectsCache_");
                    m_instance = newGO.AddComponent<UnityObjectsCache>();
                }
                return m_instance;
            }
        }

        private Dictionary<GameObject, List<GameObject>> m_pool = new Dictionary<GameObject, List<GameObject>>();
        private Thread m_checkerThread;
        private bool m_isPlaying;
        private static UnityObjectsCache m_instance;

        private void Awake()
        {
            if (m_instance == null)
                m_instance = this;
            m_checkerThread = new Thread(_checkerWorker);
            m_checkerThread.Start();
        }
        private void Update()
        {
            m_isPlaying = Application.isPlaying;
        }
        private void _checkerWorker()
        {
            while (m_isPlaying)
            {
                foreach (var item in m_pool)
                {
                    if (item.Key == null)
                    {
                        m_pool.Remove(item.Key);
                        break;
                    }
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        if (item.Value[i] == null)
                        {
                            item.Value.RemoveAt(i);
                            break;
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

        public static GameObject InstantiateObject(GameObject go, bool activateObject = true)
        {
            var pool = _instance.m_pool;
            if (pool.ContainsKey(go))
            {
                var list = pool[go];
                foreach (var obj in list)
                {
                    if (obj && obj.transform.parent == _instance.transform)
                    {
                        if (activateObject)
                        {
                            obj.SetActive(true);
                            obj.transform.SetParent(null);
                        }
                        return obj;
                    }
                };
                var newObj = Instantiate(go);
                if (activateObject)
                    newObj.SetActive(true);
                list.Add(newObj);
                return newObj;
            }
            var newObjInst = Instantiate(go);
            if (activateObject)
                newObjInst.SetActive(true);
            var newList = new List<GameObject>();
            newList.Add(newObjInst);
            pool.Add(go, newList);
            return newObjInst;
        }

        public static T InstantiateObject<T>(T comp) where T : Component
        {
            if (comp == null)
                return null;
            var compObj = comp.gameObject;
            GameObject instObj = null;
            foreach (var item in _instance.m_pool)
            {
                if (compObj == item.Key)
                {
                    instObj = InstantiateObject(item.Key, true);
                    break;
                }
                else
                {
                    foreach (var itemInst in item.Value)
                    {
                        if (itemInst == compObj)
                        {
                            instObj = InstantiateObject(item.Key, true);
                            break;
                        }
                    }
                }
            }
            if (instObj == null)
                instObj = InstantiateObject(compObj, true);
            return instObj.GetComponentCached<T>();
        }

        public static void DestroyObject(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(_instance.transform, true);
        }
    }
}
