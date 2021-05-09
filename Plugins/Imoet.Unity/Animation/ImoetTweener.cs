using System;
using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity.Animation
{
    public class ImoetTweener : MonoBehaviour {
        public float evaluatedProgress {
            get { return m_evaluatedProgress; }
            set {
                m_evaluatedProgress = Mathf.Clamp(value, 0.0f, 1.0f);
            }
        }

        [SerializeField]
        private bool m_controlled;
        [SerializeField, Range(0.0f, 1.0f)]
        private float m_evaluatedProgress;

        private List<Tweener> m_tweenList = new List<Tweener>();

        //Unity Thread
        private void Update() {
            if (!m_controlled)
            {
                foreach (var item in m_tweenList)
                    item.Update(Time.deltaTime);
            }
            else {
                Evaluate(m_evaluatedProgress);
            }
        }

        //Public Function
        public void AddTween(Tweener tweener) {
            m_tweenList.Add(tweener);
        }
        public void RemoveTween(Tweener tweener) {
            m_tweenList.Remove(tweener);
        }
        public Tweener GetTween(int idx) {
            return m_tweenList[idx];
        }

        public void StartTween() {
            if (!m_controlled)
                _execute((e) => {
                    e.StartTween();
                });
        }
        public void StartTweenReverse() {
            if (!m_controlled)
                _execute((e) => {
                    e.StartTweenReverse();
                });
        }
        public void PauseTween()
        {
            if (!m_controlled)
                _execute((e) => {
                    e.PauseTween();
                });
        }
        public void StopTween()
        {
            if (!m_controlled)
                _execute((e) => {
                    e.StopTween();
                });
        }
        public void Evaluate(float progress)
        {
           if(m_controlled)
                _execute((e) => {
                    e.Evaluate(progress);
                    e.progress = progress;
                });
        }

        //Private Function
        private void _execute(Action<Tweener> action)
        {
            foreach (var element in m_tweenList)
            {
                action?.Invoke(element);
            }
        }
    }
}
