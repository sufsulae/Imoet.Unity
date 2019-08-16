using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Imoet.Unity.UI
{
    [DisallowMultipleComponent]
    //[RequireComponent(typeof(Graphic))]
    public class PointerDispatcher : MonoBehaviour
    {
        #region Public Property
        public static PointerDispatcher instance { get; private set; }
        public List<PointerEventData> pointers
        {
            get
            {
                var l = new List<PointerEventData>();
                foreach (var ptrData in m_ptrData)
                {
                    l.Add(ptrData.Value);
                }
                return l;
            }
        }
        #endregion

        #region Private Field
        [SerializeField]
        private bool m_showRequiredComp;

        private List<PackedPointer> m_cache;
        private List<TrackedPointer> m_holdedData;
        private List<TrackedPointer> m_trackedPointer;

        private EventTrigger m_eventTrigger;
        private EventSystem m_eventSystem;
        private Canvas m_canvas;
        private Dictionary<int, PointerEventData> m_ptrData;

        private bool m_lateStart;
        private GameObject m_lastHovered;
        private Graphic m_graphic;
        #endregion

        #region Unity Thread
        private void Awake()
        {
            //Initialize
            instance = this;
            m_cache = new List<PackedPointer>();
            m_ptrData = new Dictionary<int, PointerEventData>();
            m_holdedData = new List<TrackedPointer>();
            m_trackedPointer = new List<TrackedPointer>();
            m_eventTrigger = GetComponent<EventTrigger>();
            if (!m_eventTrigger)
            {
                m_eventTrigger = gameObject.AddComponent<EventTrigger>();
                m_eventTrigger.hideFlags = m_showRequiredComp? HideFlags.NotEditable : HideFlags.HideInInspector;
            }
            m_graphic = GetComponent<Graphic>();
            if (!m_graphic)
            {
                var img = gameObject.AddComponent<Image>();
                img.color = new Color32();
                img.raycastTarget = true;
                img.hideFlags = m_showRequiredComp ? HideFlags.NotEditable : HideFlags.HideInInspector;
                m_graphic = img;
            }

            //Register Some function to EventTrigger
            //For easy management
            m_eventSystem = EventSystem.current;
            _regEntry(EventTriggerType.PointerDown, _onPointerDown);
            _regEntry(EventTriggerType.PointerUp, _onPointerUp);
            _regEntry(EventTriggerType.PointerClick, _onPointerClick);
            _regEntry(EventTriggerType.Drag, _onPointerDrag);
            _regEntry(EventTriggerType.BeginDrag, _onPointerBeginDrag);
            _regEntry(EventTriggerType.EndDrag, _onPointerEndDrag);
        }
        private void Update() {
            if (!m_lateStart) {
                //LETS HACKING, to get Raw PointerEventData from EventSystem
                var inp = (PointerInputModule)m_eventSystem.currentInputModule;
                var inpType = inp.GetType();
                var inpField = inpType.GetField("m_PointerData", BindingFlags.Instance | BindingFlags.NonPublic);
                m_ptrData = (Dictionary<int, PointerEventData>)inpField.GetValue(inp);
                m_lateStart = true;
            }

            //Replicate "On-"Event
            foreach (var ptrKeyValue in m_ptrData) {
                var ptr = ptrKeyValue.Value;
                //m_eventSystem.RaycastAll(ptr, m_rayResult);

                //Check Pointer Validation
                var ptrObj = _getPointerObj(ptr);
                if (m_lastHovered != ptr.pointerEnter)
                {
                    if(ptr.pointerEnter == gameObject)
                        _executeObjMethod(ptrObj, ptr, "OnPointerExit");
                    m_lastHovered = ptr.pointerEnter;
                    continue;
                }
                if (m_lastHovered != gameObject)
                    continue;

                //if We have a valid Pointer, Process it
                if (m_trackedPointer.Count > 0)
                {
                    //Replicate OnEnter, OnExit and OnMoved Function
                    foreach (var trackedPtr in m_trackedPointer) {
                        if (trackedPtr.data == ptr)
                        {
                            if (trackedPtr.obj != ptrObj)
                            {
                                if (!ptrObj)
                                    _executeObjMethod(trackedPtr.obj, ptr, "OnPointerExit");
                                else
                                {
                                    _executeObjMethod(trackedPtr.obj, ptr, "OnPointerExit");
                                    _executeObjMethod(ptrObj, ptr, "OnPointerEnter");
                                }
                                trackedPtr.obj = ptrObj;
                            }
                            else {
                                if (ptrObj) {
                                    if (ptr.delta != Vector2.zero)
                                        _executeObjMethod(ptrObj, ptr, "OnPointerMoved");
                                }    
                            }
                        }
                    }
                }
                else {
                    var newTrackedPtr = new TrackedPointer();
                    newTrackedPtr.obj = ptrObj;
                    newTrackedPtr.data = ptr;
                    m_trackedPointer.Add(newTrackedPtr);

                    if (newTrackedPtr.obj)
                        _executeObjMethod(newTrackedPtr.obj, ptr, "OnPointerEnter");
                }

                //Replicate OnHovered Function
                if (ptrObj != null && m_lastHovered == gameObject && !ptr.eligibleForClick && !_isObjectAlreadyHolded(ptrObj))
                    _executeObjMethod(ptrObj, ptr, "OnPointerHovered");
            }

            //Replicate OnPointerCanceled Function
            if (m_holdedData.Count > 0) {
                bool isSafe = true;
                foreach (var ptrData in m_ptrData) {
                    if (ptrData.Value.eligibleForClick) {
                        isSafe = false;
                        break;
                    }
                }
                if (isSafe) {
                    for (int i = 0; i < m_holdedData.Count; i++) {
                        var holdedData = m_holdedData[i];
                        _executeObjMethod(holdedData.obj, null, "OnPointerCanceled");
                        m_holdedData.RemoveAt(i);
                    }
                }
            }
        }
        #endregion

        #region Listener
        private void _onPointerClick(PointerEventData data) {
            if (data.pointerEnter != gameObject)
                return;
            _executeObjMethod(_getPointerObj(data), data, "OnPointerClick");
        }

        private void _onPointerUp(PointerEventData data) {
            var tgt = _getPointerObj(data);
            if (tgt != null && m_holdedData.Count > 0) {
                for (int i = 0; i < m_holdedData.Count; i++)
                {
                    var holdedData = m_holdedData[i];
                    if (holdedData.obj == tgt && data.pointerId == holdedData.data.pointerId)
                    {
                        _executeObjMethod(holdedData.obj, data, "OnPointerUp");
                        m_holdedData.RemoveAt(i);
                    }
                }
            }
        }

        private void _onPointerDown(PointerEventData data) {
            if (data.pointerEnter != gameObject)
                return;
            var tgt = _getPointerObj(data);
            _executeObjMethod(_getPointerObj(data), data, "OnPointerDown");
            if (m_holdedData != null)
                m_holdedData = new List<TrackedPointer>();
            if (tgt != null) {
                var trackedPoint = new TrackedPointer();
                trackedPoint.obj = tgt;
                trackedPoint.data = data;
                m_holdedData.Add(trackedPoint);
            }
        }

        private void _onPointerDrag(PointerEventData data) {
            if (data.pointerEnter != gameObject)
                return;
            foreach (var holdedData in m_holdedData) {
                if (holdedData.data.pointerId == data.pointerId)
                    _executeObjMethod(holdedData.obj, data, "OnPointerDrag");
            }
        }

        private void _onPointerBeginDrag(PointerEventData data) {
            if (data.pointerEnter != gameObject)
                return;
            foreach (var holdedData in m_holdedData) {
                if (holdedData.data.pointerId == data.pointerId)
                    _executeObjMethod(holdedData.obj, data, "OnPointerBeginDrag");
            }
        }

        private void _onPointerEndDrag(PointerEventData data) {
            if (data.pointerEnter != gameObject)
                return;
            foreach (var holdedData in m_holdedData) {
                if (holdedData.data.pointerId == data.pointerId)
                    _executeObjMethod(holdedData.obj, data, "OnPointerEndDrag");
            }
        }
        #endregion

        #region Private Method
        private void _regEntry(EventTriggerType type, Action<PointerEventData> callback) {
            var entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener((data) => { callback((PointerEventData)data); });
            m_eventTrigger.triggers.Add(entry);
        }

        private GameObject _getPointerObj(PointerEventData data, bool checkPressed = false) {
            Ray ray;
            if (checkPressed)
                ray = Camera.main.ScreenPointToRay(data.pressPosition);
            else
                ray = Camera.main.ScreenPointToRay(data.position);
            GameObject tgt = null;

            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit) && rayHit.collider)
            {
                tgt = rayHit.collider.gameObject;
            }

            if (tgt == null)
            {
                var rayHit2D = Physics2D.Raycast(ray.origin, ray.direction);
                if (rayHit2D.collider)
                    tgt = rayHit2D.collider.gameObject;
            }

            return tgt;
        }

        private void _executeObjMethod(GameObject obj, PointerEventData ptrData, string methodName) {
            if (obj == null)
                return;

            var objComps = obj.GetComponents<Component>();
            var compsPtr = new List<PointerReciever>();
            var tgtT = typeof(PointerReciever);

            foreach (var objComp in objComps)
            {
                var objCompT = objComp.GetType();
                if (objComp is PointerReciever || objCompT.IsAssignableFrom(tgtT))
                {
                    var objCompPtr = (PointerReciever)objComp;
                    var objCompPtrT = objCompPtr.GetType();
                    var objCompPtrTBase = objCompPtrT;
                    while (objCompPtrTBase != tgtT) {
                        objCompPtrTBase = objCompPtrTBase.BaseType;
                    }
                    var objCompFieldDisp = objCompPtrTBase.GetField("m_dispatcher", BindingFlags.Instance | BindingFlags.NonPublic);
                    objCompFieldDisp.SetValue(objCompPtr, this);
                    compsPtr.Add((PointerReciever)objComp);
                }
            }

            bool isCached = false;
            foreach (var tgtObj in m_cache) {
                if (tgtObj.obj == obj) {
                    isCached = true;
                    foreach (var compPtr in compsPtr) {
                        if (tgtObj.methods.ContainsKey(compPtr))
                        {
                            var methods = tgtObj.methods[compPtr];
                            bool hasMethod = false;
                            foreach (var method in methods)
                            {
                                if (method.Name == methodName)
                                {
                                    method.Invoke(compPtr, new object[] { ptrData });
                                    hasMethod = true;
                                }
                            }
                            if (hasMethod) {
                                continue;
                            }

                            var compMethod = compPtr.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                            if (compMethod != null)
                            {
                                methods.Add(compMethod);
                                compMethod.Invoke(compPtr, new object[] { ptrData });
                            }

                        }
                        else {
                            var methods = new List<MethodInfo>();
                            tgtObj.methods.Add(compPtr,methods);

                            var compMethod = compPtr.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                            if (compMethod != null)
                            {
                                methods.Add(compMethod);
                                compMethod.Invoke(compPtr, new object[] { ptrData });
                            }
                        }
                    }
                    return;
                }
            }
            if (!isCached) {
                var newPack = new PackedPointer();
                newPack.obj = obj;
                foreach (var comp in compsPtr) {
                    var newMethods = new List<MethodInfo>();
                    var compMethod = comp.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                    if (compMethod != null) {
                        newMethods.Add(compMethod);
                        compMethod.Invoke(comp, new object[] { ptrData });
                    }
                    newPack.methods.Add(comp, newMethods);
                }
            }
        }

        private bool _isObjectAlreadyHolded(GameObject gameobject) {
            var holded = false;
            for (int i = 0; i < m_holdedData.Count; i++)
            {
                if (m_holdedData[i].obj == gameobject) {
                    holded = true;
                    break;
                }
                    
            }
            return holded;
        }
        #endregion

        #region Class
        class TrackedPointer {
            public GameObject obj;
            public PointerEventData data;
        }
        class PackedPointer {
            public GameObject obj;
            public Dictionary<PointerReciever, List<MethodInfo>> methods;
            public PackedPointer() {
                methods = new Dictionary<PointerReciever, List<MethodInfo>>();
            }
        }
        #endregion
    }
}