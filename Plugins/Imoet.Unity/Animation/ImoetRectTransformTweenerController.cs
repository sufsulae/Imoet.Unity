using UnityEngine;

namespace Imoet.Unity.Animation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class ImoetRectTransformTweenerController : MonoBehaviour {
        [SerializeField,ReadOnly]
        internal ImoetRectTransformTweener m_controller;
        private RectTransform m_rectTransform;

        public ImoetRectTransformTweener controller { get { return m_controller; } }
        public RectTransform rectTransform {
            get {
                if (!m_rectTransform)
                    m_rectTransform = GetComponent<RectTransform>();
                return m_rectTransform;
            }
        }

        private void Update() {
            if (m_controller == null)
                DestroyImmediate(gameObject);
        }
    }
}