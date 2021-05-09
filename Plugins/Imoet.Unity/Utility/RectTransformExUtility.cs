using UnityEngine;

namespace Imoet.Unity.Utility
{
    public static class RectTransformExUtility
    {
        private static Vector3[] _tempCorner;
        static RectTransformExUtility() {
            _tempCorner = new Vector3[4];
        }

        public static RectTransform GetRectTransform(this GameObject g) {
            if (g)
                return g.GetComponent<RectTransform>();
            return null;
        }

        public static RectTransform GetParentRect(this RectTransform v)
        {
            if (v.parent)
                return v.parent.GetComponent<RectTransform>();
            return null;
        }

        public static void AutoSetAnchor(this RectTransform r)
        {
            var pRect = r.GetParentRect();
            if (pRect == null)
                return;
            var _offsetMin = r.offsetMin;
            var _offsetMax = r.offsetMax;
            var _anchorMin = r.anchorMin;
            var _anchorMax = r.anchorMax;
            var pRectRect = pRect.rect;
            var anchorMin = new Vector2(
                    _anchorMin.x + (_offsetMin.x / pRectRect.width),
                    _anchorMin.y + (_offsetMin.y / pRectRect.height)
                );
            var anchorMax = new Vector2(
                    _anchorMax.x + (_offsetMax.x / pRectRect.width),
                    _anchorMax.y + (_offsetMax.y / pRectRect.height)
                );
            r.anchorMin = anchorMin;
            r.anchorMax = anchorMax;
            r.offsetMin = r.offsetMax = Vector2.zero;
        }

        /// <summary>
        ///     Get the 'Width' and 'Height' from active <see cref="RectTransform" />
        /// </summary>
        /// <returns>Vector2 with x value as 'width' and y value as 'height'</returns>
        public static Vector2 GetWorldSize(this RectTransform rect)
        {
            var corners = _getWorldCorners(rect);
            return corners.size;
        }

        /// <summary>
        ///     Get <see cref="Imoet.UI.RectCorner3D" /> from this active <see cref="RectTransform" />
        /// </summary>
        /// <returns>
        ///     <see cref="Imoet.UI.RectConrner3D" />
        /// </returns>
        public static RectCorner GetWorldCorner3D(this RectTransform rect)
        {
            return _getWorldCorners(rect);
        }

        public static RectCorner GetLocalCorner3D(this RectTransform rect)
        {
            return _getLocalCorners(rect);
        }

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

        // Private Function
        private static RectCorner _getWorldCorners(RectTransform rect)
        {
            var corners = new UnityEngine.Vector3[4];
            rect.GetWorldCorners(corners);
            return new RectCorner(corners[0], corners[2], corners[1], corners[3]);
        }

        private static RectCorner _getLocalCorners(RectTransform rect)
        {
            var corners = new UnityEngine.Vector3[4];
            rect.GetLocalCorners(corners);
            return new RectCorner(corners[0], corners[2], corners[1], corners[3]);
        }
    }
}