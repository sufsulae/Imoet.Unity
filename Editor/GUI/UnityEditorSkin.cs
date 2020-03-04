using UnityEditor;
using UnityEngine;

namespace Imoet.UnityEditor
{
    public static class UnityEditorSkin
    {
        public static GUIStyle GetInternalStyle(string name)
        {
            GUIStyle g = name;
            return g;
        }

        public static GUIStyle windowBottomResize { get { return GetInternalStyle("WindowBottomResize"); } }

        public static GUIStyle helpBox { get { return new GUIStyle(EditorStyles.helpBox); } }
        public static GUIStyle windowCloseButton { get { return new GUIStyle(GetInternalStyle("WinBtnClose")); } }
        public static GUIStyle windowMinButton { get { return new GUIStyle(GetInternalStyle("WinBtnMin")); } }
        public static GUIStyle windowMaxButton { get { return new GUIStyle(GetInternalStyle("WinBtnMax")); } }

        public static GUIStyle RLdraggingHandle { get { return new GUIStyle(GetInternalStyle("RL DragHandle")); } }
        public static GUIStyle RLheaderBackground { get { return new GUIStyle(GetInternalStyle("RL Header")); } }
        public static GUIStyle RLfooterBackground { get { return new GUIStyle(GetInternalStyle("RL Footer")); } }
        public static GUIStyle RLboxBackground { get { return new GUIStyle(GetInternalStyle("RL Background")); } }
        public static GUIStyle RLpreButton { get { return new GUIStyle(GetInternalStyle("RL FooterButton")); } }
        public static GUIStyle RLelementBackground { get { return new GUIStyle(GetInternalStyle("RL Element")); } }

        public static GUIStyle TEtoolBar { get { return new GUIStyle(GetInternalStyle("TE Toolbar")); } }
        public static GUIStyle INlockButton { get { return new GUIStyle(GetInternalStyle("IN LockButton")); } }
        public static GUIStyle invisibleButton { get { return new GUIStyle(GetInternalStyle("InvisibleButton")); } }

        public static GUIStyle centeredLabel
        {
            get
            {
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.alignment = TextAnchor.UpperCenter;
                return style;
            }
        }

        public static GUIStyle midBoldLabel
        {
            get
            {
                GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                style.alignment = TextAnchor.UpperCenter;
                return style;
            }
        }

        public static GUIStyle normalStyle
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.border = new RectOffset(6, 6, 6, 4);
                style.margin = new RectOffset(4, 4, 4, 4);
                style.padding = new RectOffset(6, 6, 4, 4);
                return style;
            }
        }
    }

    public static class UnityEditorRes
    {
        public static GUIContent GetGUIContent(Object obj, System.Type t)
        {
            return EditorGUIUtility.ObjectContent(obj, t);
        }

        public static GUIContent GetIconContent(string name)
        {
            return EditorGUIUtility.IconContent(name);
        }

        public static GUIContent GUIContentTransform
        {
            get { return GetGUIContent(null, typeof(Transform)); }
        }

        public static GUIContent IconToolbarMinus
        {
            get { return GetIconContent("Toolbar Minus"); }
        }

        public static GUIContent IconToolbarPlus
        {
            get { return GetIconContent("Toolbar Plus"); }
        }

        public static GUIContent PlayButton
        {
            get { return GetIconContent("PlayButton"); }
        }

        public static GUIContent PauseButton
        {
            get { return GetIconContent("PauseButton"); }
        }

        public static GUIContent StepButton
        {
            get { return GetIconContent("StepButton"); }
        }

        public static GUIContent PlayButtonProfile
        {
            get { return GetIconContent("PlayButtonProfile"); }
        }

        public static GUIContent PlayButtonOn
        {
            get { return GetIconContent("PlayButton On"); }
        }

        public static GUIContent PauseButtonOn
        {
            get { return GetIconContent("PauseButton On"); }
        }

        public static GUIContent StepButtonOn
        {
            get { return GetIconContent("StepButton On"); }
        }

        public static GUIContent PlayButtonProfileOn
        {
            get { return GetIconContent("PlayButtonProfile On"); }
        }

        public static GUIContent PlayButtonAnim
        {
            get { return GetIconContent("PlayButton Anim"); }
        }

        public static GUIContent PauseButtonAnim
        {
            get { return GetIconContent("PauseButton Anim"); }
        }

        public static GUIContent StepButtonAnim
        {
            get { return GetIconContent("StepButton Anim"); }
        }

        public static GUIContent PlayButtonProfileAnim
        {
            get { return GetIconContent("PlayButtonProfile Anim"); }
        }

        public static GUIContent ViewToolOrbit
        {
            get { return GetIconContent("ViewToolOrbit"); }
        }

        public static GUIContent ViewToolMove
        {
            get { return GetIconContent("ViewToolMove"); }
        }

        public static GUIContent ViewToolZoom
        {
            get { return GetIconContent("ViewToolZoom"); }
        }

        public static GUIContent ViewToolOrbitOn
        {
            get { return GetIconContent("ViewToolOrbit On"); }
        }

        public static GUIContent ViewToolMoveOn
        {
            get { return GetIconContent("ViewToolMove On"); }
        }

        public static GUIContent ViewToolZoomOn
        {
            get { return GetIconContent("ViewToolZoom On"); }
        }

        public static GUIContent MoveTool
        {
            get { return GetIconContent("MoveTool"); }
        }

        public static GUIContent RotateTool
        {
            get { return GetIconContent("RotateTool"); }
        }

        public static GUIContent ScaleTool
        {
            get { return GetIconContent("ScaleTool"); }
        }

        public static GUIContent RectTool
        {
            get { return GetIconContent("RectTool"); }
        }

        public static GUIContent MoveToolOn
        {
            get { return GetIconContent("MoveTool On"); }
        }

        public static GUIContent RotateToolOn
        {
            get { return GetIconContent("RotateTool On"); }
        }

        public static GUIContent ScaleToolOn
        {
            get { return GetIconContent("ScaleTool On"); }
        }

        public static GUIContent RectToolOn
        {
            get { return GetIconContent("RectTool On"); }
        }

        public static GUIContent ToolHandleLocal
        {
            get { return GetIconContent("ToolHandleLocal"); }
        }

        public static GUIContent ToolHandleGlobal
        {
            get { return GetIconContent("ToolHandleGlobal"); }
        }

        public static GUIContent ToolHandleCenter
        {
            get { return GetIconContent("ToolHandleCenter"); }
        }

        public static GUIContent ToolHandlePivot
        {
            get { return GetIconContent("ToolHandlePivot"); }
        }

        public static GUIContent ToolbarLayers
        {
            get { return GetIconContent("ToolbarLayers"); }
        }

        public static GUIContent OrbitViewCursor
        {
            get
            {
                GUIContent content = new GUIContent();
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(System.IO.File.ReadAllBytes(EditorApplication.applicationContentsPath + "/Resources" + "\\OrbitView.png"));
                content.image = tex;
                return content;
            }
        }
    }
}