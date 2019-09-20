using Imoet.Unity.Animation;
using Imoet.Unity.Utility;
using UnityEditor;
using UnityEngine;

namespace Imoet.UnityEditor
{
    [CustomEditor(typeof(ImoetRectTransformTweenerController))]
    public class ImoetRectTransformTweenerControllerEditor : Editor
    {
        private ImoetRectTransformTweenerController tgt;
        void OnEnable() {
            tgt = (ImoetRectTransformTweenerController)target;
        }
        void OnSceneGUI()
        {
            var tgtCorner = tgt.rectTransform.GetWorldCorner3D();
            var controllerCorner = tgt.controller.rectTransform.GetWorldCorner3D();

            Color handleTempColor = Handles.color;

            //Draw targetCorner
            Handles.color = Color.yellow;
            Handles.DrawPolyLine(tgtCorner.TopLeft, tgtCorner.BottomLeft, tgtCorner.BottomRight, tgtCorner.TopRight, tgtCorner.TopLeft);
            Handles.DrawPolyLine(controllerCorner.TopLeft, controllerCorner.BottomLeft, controllerCorner.BottomRight, controllerCorner.TopRight, controllerCorner.TopLeft);

            //Draw Bridge
            Handles.color = Color.blue;
            Handles.DrawDottedLine(tgt.rectTransform.position, tgt.controller.rectTransform.position, 10);

            Handles.color = handleTempColor;
        }
    }
}