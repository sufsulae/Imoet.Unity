#pragma warning disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace Imoet.Unity.Events
{
    public enum EventExecutionMode
    {
        AllInOne,
        OneByOne
    }
    public delegate Coroutine StartCoroutineDelegate(IEnumerator enumerator);

    [System.Serializable]
    public class ImoetUnityEvent
    {
        [SerializeField]
        private EventExecutionMode m_exeMode;
        [SerializeField]
        private List<ImoetUnityEventMoethod> m_methodList = new List<ImoetUnityEventMoethod>();

        public EventExecutionMode executionMode { get { return m_exeMode; } set { m_exeMode = value; } }
        public StartCoroutineDelegate startCoroutineDelegate { get; set; }
        public bool isExecuting { get { return m_executing; } }

        private bool m_executing;

        public void Invoke() {
            Invoke(true);
        }
        public void Invoke(int i) {
            Invoke(i, false);
        }
        public void InvokeAll() {
            InvokeAll(false);
        }
        public ImoetUnityEventMoethod GetMethod(int i) {
            return m_methodList[i];
        }
        public ImoetUnityEventMoethod[] GetMethods() {
            return m_methodList.ToArray();
        }
        public ImoetUnityEventMoethod AddEvent(Action action) {
            var newMethod = new ImoetUnityEventMoethod();
            newMethod.enable = true;
            newMethod.internalMethod = action.Method;
            newMethod.internalObject = (UnityEngine.Object)action.Target;
            m_methodList.Add(newMethod);
            return newMethod;
        }
        public bool RemoveEvent(ImoetUnityEventMoethod eventMethod) {
            return m_methodList.Remove(eventMethod);
        }
        public ImoetUnityEventMoethod InsertEvent(int index, Action action) {
            var newMethod = new ImoetUnityEventMoethod();
            newMethod.enable = true;
            newMethod.internalMethod = action.Method;
            newMethod.internalObject = (UnityEngine.Object)action.Target;
            m_methodList.Insert(index, newMethod);
            return newMethod;
        }
        public void Invoke(bool force) {
            if (!force && m_executing)
                return;

            _prepareAllItem();
            switch (m_exeMode)
            {
                case EventExecutionMode.AllInOne:
                    for (int i = 0; i < m_methodList.Count; i++)
                    {
                        if (startCoroutineDelegate != null)
                            startCoroutineDelegate(_executeSelected(i));
                        else
                            _executeNonCoroutine(i);
                    }
                    break;
                case EventExecutionMode.OneByOne:
                    if (startCoroutineDelegate != null)
                        startCoroutineDelegate(_executeOneByOne());
                    else
                        _executeAll();
                    break;
            }
        }
        public void Invoke(int i, bool force)
        {
            if (!force && m_executing)
                return;
            m_methodList[i].m_executed = false;
            if (startCoroutineDelegate != null)
                startCoroutineDelegate(_executeSelected(i));
            else
                _executeNonCoroutine(i);
        }

        public void InvokeAll(bool force)
        {
            if (!force && m_executing)
                return;

            _prepareAllItem();
            for (int i = 0; i < m_methodList.Count; i++)
                Invoke(i);
        }

        private void _prepareAllItem()
        {
            foreach (ImoetUnityEventMoethod method in m_methodList)
                method.m_executed = false;
        }
        private IEnumerator _executeOneByOne()
        {
            m_executing = true;
            for (int i = 0; i < m_methodList.Count; i++)
            {
                if (m_methodList[i].enable && !string.IsNullOrEmpty(m_methodList[i].m_methodName))
                {
                    yield return new WaitForSeconds(m_methodList[i].delay);
                    m_methodList[i].Invoke();
                    yield return m_methodList[i].isExecuted;
                }
            }
            m_executing = false;
        }
        private IEnumerator _executeSelected(int index)
        {
            m_executing = true;
            if (m_methodList[index].enable && !string.IsNullOrEmpty(m_methodList[index].m_methodName))
            {
                yield return new WaitForSeconds(m_methodList[index].delay);
                m_methodList[index].Invoke();
                yield return m_methodList[index].isExecuted;
            }
            m_executing = false;
        }
        private void _executeNonCoroutine(int index)
        {
            if (m_methodList[index].enable)
                m_methodList[index].Invoke();
        }
        private void _executeAll()
        {
            for (int i = 0; i < m_methodList.Count; i++)
            {
                if (m_methodList[i].enable && !string.IsNullOrEmpty(m_methodList[i].m_methodName))
                {
                    m_methodList[i].Invoke();
                }
            }
        }
    }

    [System.Serializable]
    public class ImoetUnityEvent<T> : ImoetUnityEvent {

    }
}
