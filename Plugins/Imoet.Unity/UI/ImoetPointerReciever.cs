using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Imoet.Unity.UI;
namespace Imoet.Unity {
    public abstract class ImoetPointerReciever : MonoBehaviour
    {
        protected ImoetPointerDispatcher dispatcher { get; } = null;
        private ImoetPointerEventDataEvent[] m_pointerEventDelegate = new ImoetPointerEventDataEvent[12];

        public ImoetPointerEventDataEvent onPointerClick { get { return _getEvent(0); } }
        public ImoetPointerEventDataEvent onPointerEnter { get { return _getEvent(1); } }
        public ImoetPointerEventDataEvent onPointerExit { get { return _getEvent(2); } }
        public ImoetPointerEventDataEvent onPointerDown { get { return _getEvent(3); } }
        public ImoetPointerEventDataEvent onPointerUp { get { return _getEvent(4); } }
        public ImoetPointerEventDataEvent onPointerHovered { get { return _getEvent(5); } }
        public ImoetPointerEventDataEvent onPointerMoved { get { return _getEvent(6); } }
        public ImoetPointerEventDataEvent onPointerDragged { get { return _getEvent(7); } }
        public ImoetPointerEventDataEvent onPointerDropped { get { return _getEvent(8); } }
        public ImoetPointerEventDataEvent onPointerBeginDragged { get { return _getEvent(9); } }
        public ImoetPointerEventDataEvent onPointerEndDragged { get { return _getEvent(10); } }
        public ImoetPointerEventDataEvent onPointerCanceled { get { return _getEvent(11); } }

        protected virtual void OnPointerClick(PointerEventData data) { onPointerClick.Invoke(data); }
        protected virtual void OnPointerEnter(PointerEventData data) { onPointerEnter.Invoke(data); }
        protected virtual void OnPointerExit(PointerEventData data) { onPointerExit.Invoke(data); }
        protected virtual void OnPointerDown(PointerEventData data) { onPointerDown.Invoke(data); }
        protected virtual void OnPointerUp(PointerEventData data) { onPointerUp.Invoke(data); }
        protected virtual void OnPointerHovered(PointerEventData data) { onPointerHovered.Invoke(data); }
        protected virtual void OnPointerMoved(PointerEventData data) { onPointerMoved.Invoke(data); }
        protected virtual void OnPointerDragged(PointerEventData data) { onPointerDragged.Invoke(data); }
        protected virtual void OnPointerDropped(PointerEventData data) { onPointerDropped.Invoke(data); }
        protected virtual void OnPointerBeginDragged(PointerEventData data) { onPointerBeginDragged.Invoke(data); }
        protected virtual void OnPointerEndDragged(PointerEventData data) { onPointerEndDragged.Invoke(data); }
        protected virtual void OnPointerCanceled(PointerEventData data) { onPointerCanceled.Invoke(data); }

        private ImoetPointerEventDataEvent _getEvent(int id) {
            if (m_pointerEventDelegate[id] == null)
                m_pointerEventDelegate[id] = new ImoetPointerEventDataEvent();
            return m_pointerEventDelegate[id];
        }
    }
    [System.Serializable]
    public class ImoetPointerEventDataEvent : UnityEvent<PointerEventData> { }
}