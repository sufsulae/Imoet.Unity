using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEventDispatcher : MonoBehaviour
{
    [SerializeField]
    private OnColliderEvent m_onEnter;
    [SerializeField]
    private OnColliderEvent m_onStay;
    [SerializeField]
    private OnColliderEvent m_onExit;

    [SerializeField]
    private OnColliderEvent2D m_onEnter2D;
    [SerializeField]
    private OnColliderEvent2D m_onStay2D;
    [SerializeField]
    private OnColliderEvent2D m_onExit2D;

    public OnColliderEvent onEnter { get { return m_onEnter; } }
    public OnColliderEvent onStay { get { return m_onStay; } }
    public OnColliderEvent onExit { get { return m_onExit; } }

    public OnColliderEvent2D onEnter2D { get { return m_onEnter2D; } }
    public OnColliderEvent2D onStay2D { get { return m_onStay2D; } }
    public OnColliderEvent2D onExit2D { get { return m_onExit2D; } }

    public List<GameObject> detectedObjects { get; private set; }

    protected virtual void Awake() {
        detectedObjects = new List<GameObject>();
    }

    protected virtual void OnTriggerEnter(Collider other) {
        m_onEnter.Invoke(other);
        if (!detectedObjects.Contains(other.gameObject))
            detectedObjects.Add(other.gameObject);
    }
    protected virtual void OnTriggerStay(Collider other) {
        m_onStay.Invoke(other);
        if (!detectedObjects.Contains(other.gameObject))
            detectedObjects.Add(other.gameObject);
    }
    protected virtual void OnTriggerExit(Collider other) {
        m_onExit.Invoke(other);
        if (detectedObjects.Contains(other.gameObject))
            detectedObjects.Remove(other.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        m_onEnter2D.Invoke(other);
        if (!detectedObjects.Contains(other.gameObject))
            detectedObjects.Add(other.gameObject);
    }
    protected virtual void OnTriggerStay2D(Collider2D other) {
        m_onStay2D.Invoke(other);
        if (!detectedObjects.Contains(other.gameObject))
            detectedObjects.Add(other.gameObject);
    }
    protected virtual void OnTriggerExit2D(Collider2D other) {
        m_onExit2D.Invoke(other);
        if (detectedObjects.Contains(other.gameObject))
            detectedObjects.Remove(other.gameObject);
    }

    protected virtual void OnCollisionEnter(Collision other) {
        m_onEnter.Invoke(other.collider);
        if (!detectedObjects.Contains(other.gameObject))
            detectedObjects.Add(other.gameObject);
    }
    protected virtual void OnCollisionStay(Collision other) {
        m_onStay.Invoke(other.collider);
        if (!detectedObjects.Contains(other.gameObject))
            detectedObjects.Add(other.gameObject);
    }
    protected virtual void OnCollisionExit(Collision other) {
        m_onExit.Invoke(other.collider);
        if (detectedObjects.Contains(other.gameObject))
            detectedObjects.Remove(other.gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other) {
        m_onEnter2D.Invoke(other.collider);
        if (!detectedObjects.Contains(other.gameObject))
            detectedObjects.Add(other.gameObject);
    }
    protected virtual void OnCollisionStay2D(Collision2D other) {
        m_onStay2D.Invoke(other.collider);
        if (!detectedObjects.Contains(other.gameObject))
            detectedObjects.Add(other.gameObject);
    }
    protected virtual void OnCollisionExit2D(Collision2D other) {
        m_onExit2D.Invoke(other.collider);
        if (detectedObjects.Contains(other.gameObject))
            detectedObjects.Remove(other.gameObject);
    }

    public void ClearAllListener() {
        m_onEnter.RemoveAllListeners();
        m_onStay.RemoveAllListeners();
        m_onExit.RemoveAllListeners();
        m_onEnter2D.RemoveAllListeners();
        m_onStay2D.RemoveAllListeners();
        m_onExit2D.RemoveAllListeners();
    }

    [System.Serializable]
    public class OnColliderEvent : UnityEvent<Collider> { }
    [System.Serializable]
    public class OnColliderEvent2D : UnityEvent<Collider2D> { }
}
