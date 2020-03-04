using Imoet.Unity.Events;
using System.Text;
using UnityEngine;

namespace Imoet.Unity.Animation
{
    public enum TweenStringMode {
        PerCharacter
    }
    public enum TweenStringStyle {
        AutoWrite
    }
    public class ImoetStringTweener : MonoBehaviour
    {
        [System.Serializable]
        internal class _stringMethod : ImoetComponentMethod<string> { }

        [SerializeField]
        private _stringMethod m_targetText;

        [SerializeField,TextArea]
        private string m_from,m_to;

        [SerializeField]
        private TweenStringMode m_tweenStringMode;
        [SerializeField]
        private TweenStringStyle m_tweenStringStyle;

        [SerializeField]
        private TweenSetting m_tweenSetting;

        public ImoetComponentMethod<string> targetText { get { return m_targetText; } }
        public string from { get { return m_from; } set { m_from = value; } }
        public string to { get { return m_to; } set { m_to = value; } }
        public TweenStringMode tweenStringMode { get { return m_tweenStringMode; } set { m_tweenStringMode = value; } }
        public TweenStringStyle tweenStringStyle { get { return m_tweenStringStyle; } set { m_tweenStringStyle = value; } }
        public TweenSetting tweenSetting { get { return m_tweenSetting; } set { m_tweenSetting = value; } }

        private Tweener<float> m_tweener;
        private StringBuilder m_sb;

        private void Awake() {
            m_tweener = new Tweener<float>();
            m_sb = new StringBuilder();
        }
    }
}