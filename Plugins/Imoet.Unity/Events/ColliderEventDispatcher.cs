using UnityEngine;
using UnityEngine.Events;
namespace Imoet.Unity.Events
{
    public class ColliderEventDispatcher : MonoBehaviour
    {
        [SerializeField]
        private OnColliderEvent m_onEnter;
        [SerializeField]
        private OnColliderEvent m_onStay;
        [SerializeField]
        private OnColliderEvent m_onExit;

        [SerializeField]
        private OnColliderEvent2D m_onEnter2D;
        [SerializeField]
        private OnColliderEvent2D m_onStay2D;
        [SerializeField]
        private OnColliderEvent2D m_onExit2D;

        public OnColliderEvent onEnter { get { return m_onEnter; } }
        public OnColliderEvent onStay { get { return m_onStay; } }
        public OnColliderEvent onExit { get { return m_onExit; } }

        public OnColliderEvent2D onEnter2D { get { return m_onEnter2D; } }
        public OnColliderEvent2D onStay2D { get { return m_onStay2D; } }
        public OnColliderEvent2D onExit2D { get { return m_onExit2D; } }


        private void OnTriggerEnter(Collider other)
        {
            m_onEnter.Invoke(other);
        }
        private void OnTriggerStay(Collider other)
        {
            m_onStay.Invoke(other);
        }
        private void OnTriggerExit(Collider other)
        {
            m_onExit.Invoke(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            m_onEnter2D.Invoke(other);
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            m_onStay2D.Invoke(other);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            m_onExit2D.Invoke(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            m_onEnter.Invoke(other.collider);
        }
        private void OnCollisionStay(Collision other)
        {
            m_onStay.Invoke(other.collider);
        }
        private void OnCollisionExit(Collision other)
        {
            m_onExit.Invoke(other.collider);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            m_onEnter2D.Invoke(other.collider);
        }
        private void OnCollisionStay2D(Collision2D other)
        {
            m_onStay2D.Invoke(other.collider);
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            m_onExit2D.Invoke(other.collider);
        }

        [System.Serializable]
        public class OnColliderEvent : UnityEvent<Collider> { }
        [System.Serializable]
        public class OnColliderEvent2D : UnityEvent<Collider2D> { }
    }
}