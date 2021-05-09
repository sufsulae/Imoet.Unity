using System;
using UnityEngine;

public static class ScreenUtility {
    public static Vector2 ScreenSizeInWorldPoint {
        get
        {
            if (Camera.main && Camera.main.orthographic)
            {
                var left = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
                var right = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
                var up = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

                var width = right.x - left.x;
                var height = up.y - right.y;
                return new Vector2(width, height);
            }
            return default(Vector2);
        }
    }
}