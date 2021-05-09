using System;
using UnityEngine;

namespace Imoet.Unity.Animation {
    [RequireComponent(typeof(ImoetTweener),typeof(RectTransform))]
    public class ImoetRectTransformTweener : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_handleFrom;
        [SerializeField]
        private RectTransform m_handleTo;
        [SerializeField]
        private SameAsSource m_sameAsSource;
        [SerializeField]
        private bool m_showHandle;

        [SerializeField]
        private TweenSetting m_rectSetting;
        [SerializeField]
        private TweenSetting m_rotSetting;
        [SerializeField]
        private TweenSetting m_sclSetting;

        private ImoetTweener m_tweener;

        private void Awake() {
            m_tweener = GetComponent<ImoetTweener>();

            //Add All Tween
            //Basic RectTransform
            m_tweener.AddTween(new Template.TweenVector2(){
                valueFrom = m_handleFrom.anchoredPosition,
                valueTo = m_handleTo.anchoredPosition,
                settings = m_rectSetting
            });
            m_tweener.AddTween(new Template.TweenVector2() {
                valueFrom = m_handleFrom.sizeDelta,
                valueTo = m_handleTo.sizeDelta,
                settings = m_rectSetting
            });
            m_tweener.AddTween(new Template.TweenVector2() {
                valueFrom = m_handleFrom.anchorMin,
                valueTo = m_handleTo.anchorMin,
                settings = m_rectSetting
            });
            m_tweener.AddTween(new Template.TweenVector2() {
                valueFrom = m_handleFrom.anchorMax,
                valueTo = m_handleTo.anchorMax,
                settings = m_rectSetting
            });

            //Rotation
            m_tweener.AddTween(new Template.TweenVector3() {
                valueFrom = m_handleFrom.localEulerAngles,
                valueTo = m_handleTo.localEulerAngles,
                settings = m_rotSetting
            });

            //Scale
            m_tweener.AddTween(new Template.TweenVector3()
            {
                valueFrom = m_handleFrom.localScale,
                valueTo = m_handleTo.localScale,
                settings = m_sclSetting
            });
        }
    }
}