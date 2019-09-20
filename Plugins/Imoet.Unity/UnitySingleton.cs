using UnityEngine;

namespace Imoet.Unity
{
    [System.Serializable]
    public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private static T m_instance;
        public static T instance { get {
                if (m_instance == null) {
                    m_instance = (T)FindObjectOfType(typeof(T));
                    if (m_instance == null) {
                        var singletonObject = new GameObject(typeof(T).Name + "(Singleton)");
                        m_instance = singletonObject.AddComponent<T>();
                        var t = m_instance as UnitySingleton<T>;
                        t._onInstance();
                    }
                }
                return m_instance;
            }
        }
        protected virtual void _onInstance() { }
    }
}
