using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity
{
    public static class UnityCommonUtility
    {
        private static Vector3[] _tempCorner;
        static UnityCommonUtility()
        {
            _tempCorner = new Vector3[4];
        }

        #region RectTransform Utility
        public static RectCorner GetWorldCorners2D(this RectTransform tr)
        {
            tr.GetWorldCorners(_tempCorner);
            return new RectCorner(_tempCorner[1], _tempCorner[2], _tempCorner[3], _tempCorner[0]);
        }
        public static RectCorner GetLocalCorners2D(this RectTransform tr)
        {
            tr.GetLocalCorners(_tempCorner);
            return new RectCorner(_tempCorner[1], _tempCorner[2], _tempCorner[3], _tempCorner[0]);
        }
        public static RectTransform GetParentRect(this RectTransform v)
        {
            if (v.parent)
                return v.parent.GetComponent<RectTransform>();
            return null;
        }
        public static void AutoSetAnchor(this RectTransform r)
        {
            RectTransform pRect = r.GetParentRect();
            if (pRect == null)
                return;
            var _offsetMin = r.offsetMin;
            var _offsetMax = r.offsetMax;
            var _anchorMin = r.anchorMin;
            var _anchorMax = r.anchorMax;
            var pRectRect = pRect.rect;
            var anchorMin = new UnityEngine.Vector2(
                    _anchorMin.x + (_offsetMin.x / pRectRect.width),
                    _anchorMin.y + (_offsetMin.y / pRectRect.height)
                );
            var anchorMax = new UnityEngine.Vector2(
                    _anchorMax.x + (_offsetMax.x / pRectRect.width),
                    _anchorMax.y + (_offsetMax.y / pRectRect.height)
                );
            r.anchorMin = anchorMin;
            r.anchorMax = anchorMax;
            r.offsetMin = r.offsetMax = UnityEngine.Vector2.zero;
        }
        #endregion

        #region Object Utility
        public static T[] GetAllComponentInChildren<T>(this GameObject tr)
        {
            var result = new List<T>();
            _catchAllComponentDownward(result, tr.transform);
            return result.ToArray();
        }
        public static T[] GetAllComponentInParent<T>(this GameObject tr)
        {
            var result = new List<T>();
            _catchAllComponentUpward(result, tr.transform);
            return result.ToArray();
        }
        public static T[] GetAllComponentInChildren<T>(this Component tr)
        {
            var result = new List<T>();
            _catchAllComponentDownward(result, tr.transform);
            return result.ToArray();
        }
        public static T[] GetAllComponentInParent<T>(this Component tr)
        {
            var result = new List<T>();
            _catchAllComponentUpward(result, tr.transform);
            return result.ToArray();
        }

        public static void SetLayerRecursive(this GameObject obj, int layer)
        {
            obj.layer = layer;
            var allChilds = new List<Transform>();
            _catchAllChildTransform(allChilds, obj.transform);
            foreach (var item in allChilds)
            {
                item.gameObject.layer = layer;
            }
        }
        #endregion

        private static void _catchAllComponentDownward<T>(List<T> list, Transform start)
        {
            foreach (Transform child in start)
            {
                var comp = child.GetComponent<T>();
                if (comp != null)
                    list.Add(comp);
                if (child.childCount > 0)
                    _catchAllComponentDownward(list, child);
            }
        }
        private static void _catchAllComponentUpward<T>(List<T> list, Transform start)
        {
            if (start.parent)
            {
                var comp = start.parent.GetComponent<T>();
                if (comp != null)
                    list.Add(comp);
                _catchAllComponentUpward(list, start.parent);
            }
        }

        private static void _catchAllChildTransform(List<Transform> list, Transform start)
        {
            foreach (Transform child in start)
            {
                list.Add(child);
                if (child.childCount > 0)
                    _catchAllChildTransform(list, child);
            }
        }
    }
}
