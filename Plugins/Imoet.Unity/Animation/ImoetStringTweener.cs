using UnityEngine;
using UnityEngine.Events;
using Imoet.Unity.Events;

namespace Imoet.Unity.Animation
{
    [System.Serializable]
    internal class ImoetStringAction : UnityEvent<string> { }
    public class ImoetStringTweener : MonoBehaviour
    {
        [SerializeField]
        private ImoetComponentMethod m_action;
        [SerializeField]
        private StrComp m_actionString;

        [SerializeField,TextArea]
        private string m_from,m_to;

        [SerializeField]
        private TweenSetting m_tweenSetting;

        private Tweener<float> m_tweener;

        private void Awake() {
            m_tweener = new Tweener<float>();
        }
    }

    [System.Serializable]
    internal class StrComp : ImoetComponentMethod<string> { }
}