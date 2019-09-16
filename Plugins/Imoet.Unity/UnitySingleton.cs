using UnityEngine;

namespace Imoet.Unity
{
    public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T instance { get {
                if (m_instance == null) {
                    m_instance = (T)FindObjectOfType(typeof(T));
                    if (m_instance == null) {
                        var singletonObject = new GameObject(typeof(T).Name + "(Singleton)");
                        m_instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return m_instance;
            }
        }
    }
}
