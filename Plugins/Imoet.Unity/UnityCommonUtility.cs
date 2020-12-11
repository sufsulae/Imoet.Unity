using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity
{
    public static class UnityCommonUtility
    {
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
