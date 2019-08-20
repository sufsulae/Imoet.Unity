using UnityEngine;
using UnityEngine.EventSystems;
using Imoet.Unity.UI;
namespace Imoet.Unity {
    public abstract class PointerReciever : MonoBehaviour
    {
        private PointerDispatcher m_dispatcher = null;
        protected PointerDispatcher dispatcher { get { return m_dispatcher; } }

        protected virtual void OnPointerClick(PointerEventData data) { }
        protected virtual void OnPointerEnter(PointerEventData data) { }
        protected virtual void OnPointerExit(PointerEventData data) { }
        protected virtual void OnPointerDown(PointerEventData data) { }
        protected virtual void OnPointerUp(PointerEventData data) { }
        protected virtual void OnPointerHovered(PointerEventData data) { }
        protected virtual void OnPointerMoved(PointerEventData data) { }
        protected virtual void OnPointerDragged(PointerEventData data) { }
        protected virtual void OnPointerDropped(PointerEventData data) { }
        protected virtual void OnPointerBeginDragged(PointerEventData data) { }
        protected virtual void OnPointerEndDragged(PointerEventData data) { }
        protected virtual void OnPointerCanceled(PointerEventData data) { }
    }
}