using Imoet.Unity.Utility;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Imoet.Unity.Animation
{
    [DisallowMultipleComponent]
    public class ImoetComponentTweener : MonoBehaviour
    {
        [SerializeField,FullBarToggle]
        private bool m_autoPlay;
        [SerializeField]
        private List<Item> m_items;

        //Unity Thread
        private void Awake() {
            foreach (var item in m_items) {
                item.Initialize();
            }
        }
        private void Start() {
            if (m_autoPlay)
                foreach (var item in m_items) {
                    item.StartTween();
                }
        }

        private void Update() {
            foreach (var item in m_items) {
                item.UpdateTween(Time.deltaTime);
            }
        }

        //Public Function
        public void StartReverseTween() {
            foreach (var item in m_items)
                item.StartReverseTween();
        }
        public void StartReverseTween(int idx) {
            m_items[idx].StartReverseTween();
        }

        public void StartTween() {
            foreach (var item in m_items)
                item.StartTween();
        }
        public void StartTween(int idx) {
            m_items[idx].StartTween();
        }

        public void StopTween() {
            foreach (var item in m_items)
                item.StopTween();
        }
        public void StopTween(int idx) {
            m_items[idx].StopTween();
        }

        public void PauseTween() {
            foreach (var item in m_items)
                item.PauseTween();
        }
        public void PauseTween(int idx) {
            m_items[idx].PauseTween();
        }

        public void SwapAndStart() {
            foreach (var item in m_items)
                item.SwapAndStart();
        }
        public void SwapAndStart(int idx) {
            m_items[idx].SwapAndStart();
        }

        //Self Class
        [System.Serializable]
        internal class Item {
            //Property
            public TweenSetting setting { get { return m_setting; } set { m_setting = value; } }
            public MethodInfo methodInfo { get { return m_methodInfo; } set { m_methodInfo = value; } }

            //Serialized Field
            [SerializeField]
            private string m_methodName;
            [SerializeField]
            private Component m_component;
            [SerializeField]
            private UnityExUtility.UnityReadableType m_valueType;
            [SerializeField]
            private TweenSetting m_setting;

            //Field
            private MethodInfo m_methodInfo;
            private readonly System.Object m_tweenerObj;

            //Value
            [SerializeField]
            UnityExInternalUtilty.tweenByte val_byte;
            [SerializeField]
            UnityExInternalUtilty.tweenShort val_short;
            [SerializeField]
            UnityExInternalUtilty.tweenInt val_int;
            [SerializeField]
            UnityExInternalUtilty.tweenFloat val_float;
            [SerializeField]
            UnityExInternalUtilty.tweenDouble val_double;
            [SerializeField]
            UnityExInternalUtilty.tweenVector2 val_vector2;
            [SerializeField]
            UnityExInternalUtilty.tweenVector3 val_vector3;
            [SerializeField]
            UnityExInternalUtilty.tweenVector4 val_vector4;
            [SerializeField]
            UnityExInternalUtilty.tweenQuaternion val_quaternion;
            [SerializeField]
            UnityExInternalUtilty.tweenRect val_rect;
            [SerializeField]
            UnityExInternalUtilty.tweenColor val_color;
            [SerializeField]
            UnityExInternalUtilty.tweenColor32 val_color32;

            //Global
            ITweenVal m_tweener;

            public void Initialize() {
                m_methodInfo = m_component.GetType().GetMethod(m_methodName);
                if (m_methodInfo == null)
                {
                    Debug.LogError("Failed to find a Method: " + m_methodInfo);
                    return;
                }
                switch (m_valueType) {
                    case UnityExUtility.UnityReadableType.Byte:
                        var aB = val_byte;
                        aB.tweenerSetting = m_setting;
                        aB.Initialize();
                        aB.tweener.tweenCalc = (progress) => {
                            return (byte)(aB.valStart + (aB.valEnd - aB.valStart) * progress);
                        };
                        aB.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aB;
                        break;
                    case UnityExUtility.UnityReadableType.Short:
                        var aS = val_short;
                        aS.tweenerSetting = m_setting;
                        aS.Initialize();
                        aS.tweener.tweenCalc = (progress) => {
                            return (short)(aS.valStart + (aS.valEnd - aS.valStart) * progress);
                        };
                        aS.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aS;
                        break;
                    case UnityExUtility.UnityReadableType.Int:
                        var aI = val_int;
                        aI.tweenerSetting = m_setting;
                        aI.Initialize();
                        aI.tweener.tweenCalc = (progress) => {
                            return (int)(aI.valStart + (aI.valEnd - aI.valStart) * progress);
                        };
                        aI.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aI;
                        break;
                    case UnityExUtility.UnityReadableType.Float:
                        var aF = val_float;
                        aF.tweenerSetting = m_setting;
                        aF.Initialize();
                        aF.tweener.tweenCalc = (progress) => {
                            return (aF.valStart + (aF.valEnd - aF.valStart) * progress);
                        };
                        aF.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aF;
                        break;
                    case UnityExUtility.UnityReadableType.Double:
                        var aD = val_double;
                        aD.tweenerSetting = m_setting;
                        aD.Initialize();
                        aD.tweener.tweenCalc = (progress) => {
                            return (aD.valStart + (aD.valEnd - aD.valStart) * progress);
                        };
                        aD.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aD;
                        break;
                    case UnityExUtility.UnityReadableType.Vector2:
                        var aV2 = val_vector2;
                        aV2.tweenerSetting = m_setting;
                        aV2.Initialize();
                        aV2.tweener.tweenCalc = (progress) => {
                            return (aV2.valStart + (aV2.valEnd - aV2.valStart) * progress);
                        };
                        aV2.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aV2;
                        break;
                    case UnityExUtility.UnityReadableType.Vector3:
                        var aV3 = val_vector3;
                        aV3.tweenerSetting = m_setting;
                        aV3.Initialize();
                        aV3.tweener.tweenCalc = (progress) => {
                            return (aV3.valStart + (aV3.valEnd - aV3.valStart) * progress);
                        };
                        aV3.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aV3;
                        break;
                    case UnityExUtility.UnityReadableType.Vector4:
                        var aV4 = val_vector4;
                        aV4.tweenerSetting = m_setting;
                        aV4.Initialize();
                        aV4.tweener.tweenCalc = (progress) => {
                            return (aV4.valStart + (aV4.valEnd - aV4.valStart) * progress);
                        };
                        aV4.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aV4;
                        break;
                    case UnityExUtility.UnityReadableType.Quaternion:
                        var aQ = val_quaternion;
                        aQ.tweenerSetting = m_setting;
                        aQ.Initialize();
                        aQ.tweener.tweenCalc = (progress) => {
                            return Quaternion.Lerp(aQ.valStart, aQ.valEnd, progress);
                        };
                        aQ.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aQ;
                        break;
                    case UnityExUtility.UnityReadableType.Rect:
                        var aR = val_rect;
                        aR.tweenerSetting = m_setting;
                        aR.Initialize();
                        aR.tweener.tweenCalc = (progress) => {
                            var res = default(Rect);
                            var resStart = aR.tweener.valueStart;
                            var resEnd = aR.tweener.valueEnd;
                            res.x = Mathf.Lerp(resStart.x, resEnd.x, progress);
                            res.y = Mathf.Lerp(resStart.y, resEnd.y, progress);
                            res.width = Mathf.Lerp(resStart.width, resEnd.width, progress);
                            res.height = Mathf.Lerp(resStart.height, resEnd.height, progress);
                            return res;
                        };
                        aR.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aR;
                        break;
                    case UnityExUtility.UnityReadableType.Color:
                        var aCol = val_color;
                        aCol.tweenerSetting = m_setting;
                        aCol.Initialize();
                        aCol.tweener.tweenCalc = (progress) => {
                            return (aCol.valStart + (aCol.valEnd - aCol.valStart) * progress);
                        };
                        aCol.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aCol;
                        break;
                    case UnityExUtility.UnityReadableType.Color32:
                        var aCol32 = val_color32;
                        aCol32.tweenerSetting = m_setting;
                        aCol32.Initialize();
                        aCol32.tweener.tweenCalc = (progress) => {
                            return Color32.Lerp(aCol32.valStart, aCol32.valEnd, progress);
                        };
                        aCol32.tweener.tweenDelegate = (value) => {
                            m_methodInfo.Invoke(m_component, new object[] { value });
                        };
                        m_tweener = aCol32;
                        break;
                }
            }

            public void StartTween() {
                m_tweener.GetTweener().StartTween();
            }

            public void StopTween() {
                m_tweener.GetTweener().StopTween();
            }

            public void PauseTween() {
                m_tweener.GetTweener().PauseTween();
            }

            public void StartReverseTween() {
                m_tweener.GetTweener().StartReverseTween();
            }

            public void SwapAndStart() {
                m_tweener.GetTweener().SwapAndStart();
            }

            public void UpdateTween(float UpdateTime) {
                m_tweener.GetTweener().Update(UpdateTime);
            }
        }
    }
}

