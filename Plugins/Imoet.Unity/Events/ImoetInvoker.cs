using UnityEngine;
namespace Imoet.Unity.Events
{
    public class ImoetInvoker : MonoBehaviour
    {
        [WideToggle]
        public bool AutoExecute;
        [WideToggle]
        public bool forceExe;

        public UnityEventEx OnEvent { get { return m_OnEvent; } }

        [SerializeField]
        private UnityEventEx m_OnEvent;

        void Awake() {
            m_OnEvent.startCoroutineDelegate = StartCoroutine;
        }
        void Start() {
            if (AutoExecute)
                Invoke();
        }

        public void Invoke() {
            m_OnEvent.Invoke(forceExe);
        }
        public void Invoke(int i) {
            m_OnEvent.Invoke(i, forceExe);
        }
    }
}