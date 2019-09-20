using UnityEngine;

namespace Imoet.Unity.Animation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class ImoetRectTransformTweener : MonoBehaviour {
        [SerializeField]
        ImoetRectTransformTweenerController m_tgtRectTransformCtrl;

        [SerializeField,ReadOnly]
        private RectTransform
            m_myRectTransform,
            m_tgtRectTransform;
        [SerializeField]
        private bool
            m_tweenRectEnabled,
            m_tweenRotEnabled,
            m_tweenSclEnabled;
        [SerializeField]
        private TweenSetting
            m_tweenRectSetting,
            m_tweenRotSetting,
            m_tweenSclSetting;

        private TweenVector2
            m_tweenAnchorMin,
            m_tweenAnchorMax,
            m_tweenDeltaSize;
        private TweenVector3
            m_tweenPosition,
            m_tweenRotation,
            m_tweenScale;

        private RectTransform m_rtransform;

        public ImoetRectTransformTweenerController controlledRect { get { return m_tgtRectTransformCtrl; } }
        public RectTransform rectTransform { get {
                if (!m_rtransform)
                    m_rtransform = GetComponent<RectTransform>();
                return m_rtransform;
            }
        }
        private TweenVector2[] _tweenV2Packed { get { return new TweenVector2[] { m_tweenAnchorMin, m_tweenAnchorMax, m_tweenDeltaSize }; } }
        private TweenVector3[] _tweenV3Packed { get { return new TweenVector3[] { m_tweenPosition, m_tweenRotation, m_tweenScale }; } }
        private ITweener[] _tweenPacked {
            get {
                return new ITweener[] {
                    m_tweenAnchorMin,
                    m_tweenAnchorMax,
                    m_tweenDeltaSize,
                    m_tweenPosition,
                    m_tweenRotation,
                    m_tweenScale
                };
            }
        }
        private ITweener[] _tweenRectPack {
            get {
                return new ITweener[] {
                    m_tweenAnchorMin,
                    m_tweenAnchorMax,
                    m_tweenDeltaSize,
                    m_tweenPosition
                };
            }
        }

        private void _copyRectTransform(RectTransform from, RectTransform to) {
            to.anchoredPosition = from.anchoredPosition;
            to.anchorMin = from.anchorMin;
            to.anchorMax = from.anchorMax;
            to.offsetMin = from.offsetMin;
            to.offsetMax = from.offsetMax;
            to.localScale = from.localScale;
            to.localEulerAngles = from.localEulerAngles;
        }

        public void BuildNewController() {
            if (m_tgtRectTransformCtrl)
                Destroy(m_tgtRectTransformCtrl.gameObject);

            var newGO = new GameObject(name + "_Controller");
            newGO.layer = gameObject.layer;

            m_tgtRectTransform = newGO.AddComponent<RectTransform>();
            m_tgtRectTransformCtrl = newGO.AddComponent<ImoetRectTransformTweenerController>();
            m_tgtRectTransformCtrl.m_controller = this;

            if (m_myRectTransform == null)
                m_myRectTransform = GetComponent<RectTransform>();

            m_tgtRectTransform.transform.SetParent(m_myRectTransform.parent, false);
            _copyRectTransform(m_myRectTransform, m_tgtRectTransform);
        }

        public void DestroyController() {
            if (m_tgtRectTransformCtrl != null) {
                m_tgtRectTransformCtrl.m_controller = null;
                m_tgtRectTransformCtrl = null;
            } 
        }

        public void Prepare() {
            m_myRectTransform = GetComponent<RectTransform>();
            m_tgtRectTransform = m_tgtRectTransformCtrl.rectTransform;

            m_tweenAnchorMin = new TweenVector2()
            {
                setting = m_tweenRectSetting,
                valueStart = m_myRectTransform.anchorMin,
                valueEnd = m_tgtRectTransform.anchorMin,
                tweenDelegate = (val) => {
                    m_myRectTransform.anchorMin = val;
                    m_tweenAnchorMin.valueEnd = m_tgtRectTransform.anchorMin;
                }
            };

            m_tweenAnchorMax = new TweenVector2()
            {
                setting = m_tweenRectSetting,
                valueStart = m_myRectTransform.anchorMax,
                valueEnd = m_tgtRectTransform.anchorMax,
                tweenDelegate = (val) => {
                    m_myRectTransform.anchorMax = val;
                    m_tweenAnchorMax.valueEnd = m_tgtRectTransform.anchorMax;
                }
            };

            m_tweenDeltaSize = new TweenVector2()
            {
                setting = m_tweenRectSetting,
                valueStart = m_myRectTransform.sizeDelta,
                valueEnd = m_tgtRectTransform.sizeDelta,
                tweenDelegate = (val) => {
                    m_myRectTransform.sizeDelta = val;
                    m_tweenDeltaSize.valueEnd = m_tgtRectTransform.sizeDelta;
                }
            };

            m_tweenPosition = new TweenVector3()
            {
                setting = m_tweenRectSetting,
                valueStart = m_myRectTransform.localPosition,
                valueEnd = m_tgtRectTransform.localPosition,
                tweenDelegate = (val) => {
                    m_myRectTransform.localPosition = val;
                    m_tweenPosition.valueEnd = m_tgtRectTransform.localPosition;
                }
            };

            m_tweenRotation = new TweenVector3()
            {
                setting = m_tweenRectSetting,
                valueStart = m_myRectTransform.localEulerAngles,
                valueEnd = m_tgtRectTransform.localEulerAngles,
                tweenDelegate = (val) => {
                    m_myRectTransform.localEulerAngles = val;
                    m_tweenRotation.valueEnd = m_tgtRectTransform.localEulerAngles;
                }
            };

            m_tweenScale = new TweenVector3() {
                setting = m_tweenRectSetting,
                valueStart = m_myRectTransform.localScale,
                valueEnd = m_tgtRectTransform.localScale,
                tweenDelegate = (val) => {
                    m_myRectTransform.localScale = val;
                    m_tweenScale.valueEnd = m_tgtRectTransform.localScale;
                }
            };
        }

        public void StartTween() {
            if (m_tweenRectEnabled)
            {
                var pack = _tweenRectPack;
                foreach (var item in pack)
                    item.StartTween();
            }
            if (m_tweenRotEnabled)
                m_tweenRotation.StartTween();
            if (m_tweenSclEnabled)
                m_tweenScale.StartTween();
        }
        public void StartReverseTween() {
            if (m_tweenRectEnabled)
            {
                var pack = _tweenRectPack;
                foreach (var item in pack)
                    item.StartReverseTween();
            }
            if (m_tweenRotEnabled)
                m_tweenRotation.StartReverseTween();
            if (m_tweenSclEnabled)
                m_tweenScale.StartReverseTween();
        }
        public void StopTween() {
            if (m_tweenRectEnabled) {
                var pack = _tweenRectPack;
                foreach (var item in pack)
                    item.StopTween();
            }
            if (m_tweenRotEnabled)
                m_tweenRotation.StopTween();
            if (m_tweenSclEnabled)
                m_tweenScale.StopTween();
        }
        public void PauseTween() {
            if (m_tweenRectEnabled)
            {
                var pack = _tweenRectPack;
                foreach (var item in pack)
                    item.PauseTween();
            }
            if (m_tweenRotEnabled)
                m_tweenRotation.PauseTween();
            if (m_tweenSclEnabled)
                m_tweenScale.PauseTween();
        }
        public void SwapAndStart() {
            if (m_tweenRectEnabled)
            {
                var pack = _tweenRectPack;
                foreach (var item in pack)
                    item.SwapAndStart();
            }
            if (m_tweenRotEnabled)
                m_tweenRotation.SwapAndStart();
            if (m_tweenSclEnabled)
                m_tweenScale.SwapAndStart();
        }

        //Unity Thread
        private void Awake() {
            Prepare();
        }
        private void Update() {
            if (m_tweenRectEnabled)
            {
                var pack = _tweenRectPack;
                foreach (var item in pack) {
                    item.Update();
                }  
            }
            if (m_tweenRotEnabled)
                m_tweenRotation.Update();
            if (m_tweenSclEnabled)
                m_tweenScale.Update();
        }
    }
}

