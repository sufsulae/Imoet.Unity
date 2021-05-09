using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity.Events
{
    public class ImoetInvokeSequence : MonoBehaviour {
        [SerializeField,WideToggle]
        private bool m_autoExec;
        [SerializeField]
        private EventExecutionMode m_mode;
        [SerializeField]
        private List<ImoetInvokSequenceItem> m_sequence;

        public void Invoke(string name) {
            foreach (var seq in m_sequence) {
                if (seq.name == name)
                    seq.events.Invoke();
            }
        }

        public void Invoke() {
            foreach (var seq in m_sequence) {
                seq.events.Invoke();
            }
        }

        public void Invoke(int idx) {
            m_sequence[idx].events.Invoke();
        }
        private void Awake() {
            foreach (var seq in m_sequence) {
                seq.events.startCoroutineDelegate = StartCoroutine;
            }
        }
        private void Start()
        {
            if (m_autoExec)
                Invoke();
        }
    }

    [System.Serializable]
    public class ImoetInvokSequenceItem
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        private ImoetUnityEvent m_event;

        public string name { get { return m_name; } }
        public ImoetUnityEvent events { get { return m_event; } }
    }
}