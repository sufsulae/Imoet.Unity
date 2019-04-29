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
        AllIn1,
        OneBy1
    }
    public delegate Coroutine StartCoroutineDelegate(IEnumerator enumerator);

    [System.Serializable]
    public class UnityEventEx
    {
        [SerializeField]
        private EventExecutionMode m_exeMode;
        [SerializeField]
        private List<UnityEventExMethod> m_methodList = new List<UnityEventExMethod>();

        public EventExecutionMode executionMode
        {
            get { return m_exeMode; }
            set { m_exeMode = value; }
        }
        public StartCoroutineDelegate startCoroutineDelegate
        {
            get; set;
        }
        public bool isExecuting
        {
            get { return m_executing; }
        }

        private bool m_executing;

        public void Invoke()
        {
            Invoke(true);
        }
        public void Invoke(int i)
        {
            Invoke(i, false);
        }
        public void InvokeAll()
        {
            InvokeAll(false);
        }
        public UnityEventExMethod GetMethod(int i)
        {
            return m_methodList[i];
        }
        public UnityEventExMethod[] GetMethods()
        {
            return m_methodList.ToArray();
        }
        public UnityEventExMethod AddEvent(Action action)
        {
            var newMethod = new UnityEventExMethod();
            newMethod.enable = true;
            newMethod.internalMethod = action.Method;
            newMethod.internalObject = (UnityEngine.Object)action.Target;
            m_methodList.Add(newMethod);
            return newMethod;
        }
        public bool RemoveEvent(UnityEventExMethod eventMethod)
        {
            return m_methodList.Remove(eventMethod);
        }
        public UnityEventExMethod InsertEvent(int index, Action action)
        {
            var newMethod = new UnityEventExMethod();
            newMethod.enable = true;
            newMethod.internalMethod = action.Method;
            newMethod.internalObject = (UnityEngine.Object)action.Target;
            m_methodList.Insert(index, newMethod);
            return newMethod;
        }
        public void Invoke(bool force)
        {
            if (!force)
            {
                if (m_executing)
                    return;
            }
            prepareAllItem();
            switch (m_exeMode)
            {
                case EventExecutionMode.AllIn1:
                    for (int i = 0; i < m_methodList.Count; i++)
                    {
                        if (startCoroutineDelegate != null)
                            startCoroutineDelegate(executeSelected(i));
                        else
                            executeNonCoroutine(i);
                    }
                    break;
                case EventExecutionMode.OneBy1:
                    if (startCoroutineDelegate != null)
                        startCoroutineDelegate(executeOneByOne());
                    else
                        executeAll();
                    break;
            }
        }
        public void Invoke(int i, bool force)
        {
            if (!force)
            {
                if (m_executing)
                    return;
            }
            m_methodList[i].m_executed = false;
            if (startCoroutineDelegate != null)
                startCoroutineDelegate(executeSelected(i));
            else
                executeNonCoroutine(i);
        }

        public void InvokeAll(bool force)
        {
            if (!force)
            {
                if (m_executing)
                    return;
            }
            prepareAllItem();
            for (int i = 0; i < m_methodList.Count; i++)
            {
                Invoke(i);
            }
        }

        private void prepareAllItem()
        {
            foreach (UnityEventExMethod method in m_methodList)
            {
                method.m_executed = false;
            }
        }
        private IEnumerator executeOneByOne()
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
        private IEnumerator executeSelected(int index)
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
        private void executeNonCoroutine(int index)
        {
            if (m_methodList[index].enable)
            {
                m_methodList[index].Invoke();
            }
        }
        private void executeAll()
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
}
