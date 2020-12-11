using UnityEngine;

namespace Imoet.Unity.Animation
{
    public class ImoetTransformTweener : MonoBehaviour {
        [SerializeField]
        private Vector3 m_from;
        [SerializeField]
        private Vector3 m_to;
        [SerializeField]
        private TweenSetting m_setting;

        private TweenVector3 m_tween;

        private void Awake() {
            m_tween = new TweenVector3();
            m_tween.tweenDelegate = (v) => {
                transform.position = v;
            };
        }

        private void Update() {
            m_tween.Update();
        }

        public void StartTween() {
            m_tween.setting = m_setting;
            m_tween.valueStart = m_from;
            m_tween.valueEnd = m_to;
            m_tween.StartTween();
        }

        public void StopTween() {
            m_tween.StopTween();
        }

        public void PauseTween() {
            m_tween.PauseTween();
        }
    }
}