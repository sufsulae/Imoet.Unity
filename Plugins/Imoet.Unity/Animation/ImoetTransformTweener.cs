using System;
using UnityEngine;

namespace Imoet.Unity.Animation
{
    [RequireComponent(typeof(ImoetTweener))]
    public class ImoetTransformTweener : MonoBehaviour {

        [SerializeField]
        private FromToSettingVector3 m_pos;
        [SerializeField]
        private FromToSettingVector3 m_rot;
        [SerializeField]
        private FromToSettingVector3 m_scl;

        private Template.TweenVector3 m_position;
        private Template.TweenVector3 m_rotation;
        private Template.TweenVector3 m_scale;
        private ImoetTweener m_tweener;

        private void Awake() {
            m_tweener = GetComponent<ImoetTweener>();

            m_position = new Template.TweenVector3();
            m_position.settings = m_pos.setting;
            m_position.valueFrom = m_pos.from;
            m_position.valueTo = m_pos.to;
            m_position.OnTweenUpdate = (v) => {
                transform.position = v;
            };

            m_rotation = new Template.TweenVector3();
            m_rotation.settings = m_rot.setting;
            m_rotation.valueFrom = m_rot.from;
            m_rotation.valueTo = m_rot.to;

            m_scale = new Template.TweenVector3();
            m_scale.settings = m_scl.setting;
            m_scale.valueFrom = m_scl.from;
            m_scale.valueTo = m_scl.to;

            m_tweener.AddTween(m_position);
            m_tweener.AddTween(m_rotation);
            m_tweener.AddTween(m_scale);
        }

        [Serializable]
        private class FromToSettingVector3 : FromToSetting<Vector3> {
            public bool m_enable;
            public SameAsSource m_sameAsSource;
            public ObjectSpace m_space;
        }
    }
}