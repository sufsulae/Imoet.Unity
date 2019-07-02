using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

#region Interface
public interface IDynamicListHeaderDrawer
{
    float GetHeaderHeight(SerializedProperty property);
    void DrawHeader(Rect rect, SerializedProperty property);
}
public interface IDynamicListItemHeaderDrawer
{
    float GetItemHeaderHeight(SerializedProperty property);
    void DrawItemHeader(Rect rect, SerializedProperty property);
}
public interface IDynamicListItemBodyDrawer
{
    float GetItemBodyHeight(SerializedProperty property);
    void DrawItemBody(Rect rect, SerializedProperty property);
}
#endregion

namespace Imoet.UnityEditor {
    public class DynamicList4 : IDynamicListHeaderDrawer, IDynamicListItemBodyDrawer, IDynamicListItemHeaderDrawer
    {
        public bool reorderable { get; set; }

        public IDynamicListHeaderDrawer drawerHeader { get; set; }
        public IDynamicListItemHeaderDrawer drawerItemHeader { get; set; }
        public IDynamicListItemBodyDrawer drawerItemBody { get; set; }

        private SerializedProperty m_prop;
        private SerializedObject m_propObj;
        private static Style m_style = null;

        private List<Item> m_items = null;
        private Rect[] m_mappedRect = null;

        private Item m_selectedItem = null;
        private Rect m_selectedRect = default(Rect);

        private bool m_sealed = false;
        private bool m_dragged = false;
        private Vector2 m_lastMousePos = default(Vector2);

        private Rect m_headerRect = default(Rect);
        private Rect m_bodyRect = default(Rect);

        //Default Drawer
        #region Default Event Drawer
        public void DrawHeader(Rect rect, SerializedProperty property)
        {
            rect.x += 7f;
            EditorGUI.LabelField(rect, property.displayName, m_style.headerLabel);
        }
        public void DrawItemHeader(Rect rect, SerializedProperty property) {
            GUI.Label(rect, property.displayName, m_style.normalTextMiddle);
        }
        public void DrawItemBody(Rect rect, SerializedProperty property) {
            int inspectedDepth = property.depth;
            float lastHeight = 5f;
            SerializedProperty sProp = property.Copy();
            SerializedProperty eProp = property.GetEndProperty();
            foreach (SerializedProperty p in sProp)
            {
                if (SerializedProperty.EqualContents(p, eProp))
                    break;
                if (p.depth > inspectedDepth && p.depth <= inspectedDepth + 1)
                {
                    float propH = EditorGUI.GetPropertyHeight(p, null, true);
                    Rect r = new Rect(rect.x + 15, rect.y + lastHeight, rect.width - 25, propH);
                    EditorGUI.PropertyField(r, p, true);
                    lastHeight += propH + 2;
                }
            }
        }
        public float GetHeaderHeight(SerializedProperty property)  {
            return EditorGUIUtility.singleLineHeight;
        }
        public float GetItemHeaderHeight(SerializedProperty property) {
            return EditorGUIUtility.singleLineHeight;
        }
        public float GetItemBodyHeight(SerializedProperty property) {
            if (property.isExpanded)
                return EditorGUI.GetPropertyHeight(property, new GUIContent(property.displayName), true);
            return 0;
        }
        #endregion

        //Constructor
        public DynamicList4(SerializedProperty prop)
        {
            if (prop == null)
                throw new NullReferenceException("Property is Null");
            if (!prop.isArray)
                throw new ArgumentException("Property is not Array");
            m_prop = prop;
            m_propObj = prop.serializedObject;

            drawerHeader = this;
            drawerItemBody = this;
            drawerItemHeader = this;

            m_items = new List<Item>();
        }

        //Public Function
        #region Public Function
        public void Add() {
            m_prop.InsertArrayElementAtIndex(m_prop.arraySize);
            var nItem = new Item() { prop = m_prop.GetArrayElementAtIndex(m_prop.arraySize - 1), parent = this };
            nItem.prop.isExpanded = false;
            m_items.Add(nItem);
        }
        public void Delete(int idx) {
            m_prop.DeleteArrayElementAtIndex(idx);
            m_items.RemoveAt(idx);
            for (int i = 0; i < m_items.Count; i++){
                m_items[i].prop = m_prop.GetArrayElementAtIndex(i);
            }
        }
        public void Move(int src, int dst) {
            m_prop.MoveArrayElement(src, dst);

            var i = m_items[src];
            m_items[src] = m_items[dst];
            m_items[dst] = i;
        }
        public void Draw() {
            //Check if property is null
            if (m_prop == null) {
                Debug.LogError("Trying to draw nulled property, this is not allowed");
                return;
            }
               
            //Style
            if (m_style == null)
                m_style = new Style();

            //If Array Suddenly Changed, Remake
            if (m_items != null && m_items.Count != m_prop.arraySize)
            {
                var nSize = m_prop.arraySize;
                m_items = new List<Item>(nSize);
                for (int i = 0; i < nSize; i++) {
                    var nItem = new Item();
                    nItem.id = i;
                    nItem.parent = this;
                    nItem.prop = m_prop.GetArrayElementAtIndex(i);
                    m_items.Add(nItem);
                }
            }

            EditorGUILayout.Space();

            //Draw Header of List
            m_headerRect = GUILayoutUtility.GetRect(0, drawerHeader.GetHeaderHeight(m_prop) + 5.0f);
            var hRect = new Rect(m_headerRect.x, m_headerRect.y + 2.5f, m_headerRect.width, m_headerRect.height-5f);
            GUI.Box(hRect, "", m_style.header2);
            drawerHeader.DrawHeader(hRect, m_prop);
            if (GUI.Button(new Rect(hRect.width - 5f, hRect.y, hRect.height, hRect.height), m_style.plusButton, m_style.normal))
                Add();

            //Draw Body only if we have an Item
            if (m_items.Count > 0)
            {
                //Draw Background of List
                m_bodyRect = _getBodyRect();
                GUI.Box(m_bodyRect, "", m_style.body);

                //Draw All Items
                _drawItems(m_bodyRect);

                //Draw On Cue from Event
                _drawOnEvent(Event.current);
            }
            else {
                //Draw Empty
                EditorGUILayout.BeginVertical(m_style.body);
                EditorGUILayout.PrefixLabel("List Is Empty");
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space();
        }
        #endregion

        //Private Function
        #region Private Function
        private void _drawOnEvent(Event e) {
            switch (e.type) {
                case EventType.MouseDown:
                    if (!m_sealed) {
                        for (int i = 0; i < m_items.Count; i++) {
                            if (m_items[i].isHeaderContainPoint(e.mousePosition) && m_selectedItem == null) {
                                m_sealed = true;
                                m_selectedItem = m_items[i];
                                m_selectedRect = m_selectedItem.rect;
                                m_mappedRect = new Rect[m_items.Count];
                                m_lastMousePos = e.mousePosition;
                                e.Use();
                                i = 0;
                            }
                            if (m_sealed) {
                                m_mappedRect[i] = m_items[i].rect;
                            }
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (m_sealed && m_selectedItem != null) {
                        m_dragged = true;

                        //Trap Position of selected Item
                        //So its never exceed our draw area
                        m_selectedItem.rect.y = m_selectedRect.y + (e.mousePosition.y - m_lastMousePos.y);
                        var minY = m_bodyRect.y;
                        var maxY = m_bodyRect.y + m_bodyRect.height - m_selectedItem.rect.height;
                        m_selectedItem.rect.y = Mathf.Clamp(m_selectedItem.rect.y, minY, maxY);

                        var totalRect = new Rect(m_headerRect.x, m_headerRect.y, m_headerRect.width, m_headerRect.height + m_bodyRect.height);

                        var mDir = Mathf.Sign(e.delta.y);
                        if (mDir != 0) {
                            var deltaID = 0;

                            for (int i = 0; i < m_items.Count; i++)
                            {
                                if (m_items[i] != m_selectedItem)
                                {
                                    var itemRect = m_items[i].rect;
                                    if (m_selectedItem.rect.Contains(itemRect.center))
                                    {
                                        deltaID = m_items[i].id - m_selectedItem.id;
                                        break;
                                    }
                                }
                            }

                            Debug.Log("DeltaID: " + deltaID);
                            Debug.Log("Mouse Dir: " + mDir);

                            //FIXME : ReDetect if item is moving too fast
                            //if (deltaID == 0) {
                            //    for (int i = 0; i < m_items.Count; i++) {
                            //        if (m_items[i] != m_selectedItem) {
                            //            var item = m_items[i];
                            //            var itemRectCenter = item.rect.center;
                            //            var selectedItemCenter = m_selectedItem.rect.center;
                            //            if (mDir > 0) {
                            //                if (selectedItemCenter.y > itemRectCenter.y) {
                            //                    deltaID = m_items[i].id - m_selectedItem.id;
                            //                    Debug.Log("Detected item in ID: " + deltaID);
                            //                }
                            //            }
                            //            else if (mDir < 0) {
                            //                if (selectedItemCenter.y < itemRectCenter.y) {
                            //                    deltaID = m_items[i].id - m_selectedItem.id;
                            //                    Debug.Log("Detected item in ID: " + deltaID);
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            if(deltaID != 0) {
                                int dir = (int)Mathf.Sign(deltaID);
                                deltaID = (deltaID < 0) ? -deltaID : deltaID;
                                var tmpRect = default(Rect);

                                while (deltaID > 0)
                                {
                                    Item nextItem = null;
                                    for (int i = 0; i < m_items.Count; i++)
                                    {
                                        if (m_items[i].id == m_selectedItem.id + dir)
                                        {
                                            nextItem = m_items[i];
                                            break;
                                        }
                                    }
                                    tmpRect = nextItem.rect;
                                    tmpRect.y -= (m_selectedItem.rect.height) * dir;
                                    nextItem.rect = tmpRect;

                                    nextItem.id -= dir;
                                    m_selectedItem.id += dir;

                                    deltaID--;
                                }
                            }
                        }
                        e.Use();
                    } 
                    break;
                case EventType.Ignore:
                case EventType.MouseUp:
                    if (m_sealed) {
                        //Detect "Click"
                        if (!m_dragged) {
                            var pMouse = e.mousePosition;
                            var pMouseDist = Vector2.Distance(pMouse, m_lastMousePos);
                            if (pMouseDist < 0.1f)
                                m_selectedItem.prop.isExpanded = !m_selectedItem.prop.isExpanded;
                        }

                        m_dragged = false;

                        m_mappedRect = null;
                        m_selectedItem = null;
                        m_selectedRect = default(Rect);
                        m_sealed = false;
                        e.Use();
                    }
                    break;
            }
        }
        private Item _findNearestFromPoint(Vector2 point) {
            var nearest = float.MaxValue;
            Item res = null;
            for (int i = 0; i < m_items.Count; i++){
                var itemCenter = m_items[i].rect.center;
                var dist = Vector2.Distance(point, itemCenter);
                if (dist < nearest)
                {
                    nearest = dist;
                    res = m_items[i];
                }
            }
            return res;
        }
        private void _drawItems(Rect body) {
            if (m_sealed) {
                for (int i = 0; i < m_items.Count; i++) {
                    var item = m_items[i];

                    if (item == m_selectedItem)
                        continue;

                    //item.rect = m_mappedRect[i];
                    item.Draw();
                }

                m_selectedItem.Draw();
            }
            else {
                var lastH = 0.0f;
                for (int i = 0; i < m_items.Count; i++)
                {
                    var item = m_items[i];
                    var itemBodyHeight = drawerItemBody.GetItemBodyHeight(item.prop);
                    item.headerHeight = drawerItemHeader.GetItemHeaderHeight(item.prop);
                    item.rect = new Rect(body.x, body.y + lastH, body.width, item.headerHeight + itemBodyHeight);
                    item.Draw();
                    lastH += item.rect.height + 2f;
                }
            }
        }

        private Rect _getBodyRect() {
            var res = 2.0f;
            for (int i = 0; i < m_items.Count; i++)
                res += drawerItemHeader.GetItemHeaderHeight(m_items[i].prop) + drawerItemBody.GetItemBodyHeight(m_items[i].prop) + 2f;
            return GUILayoutUtility.GetRect(0, res);
        }

        private void _deleteItem(Item item) {
            int idx = m_items.IndexOf(item);
            m_prop.DeleteArrayElementAtIndex(idx);
            m_prop.serializedObject.ApplyModifiedProperties();
            m_items.RemoveAt(idx);
            for (int i = idx; i < m_items.Count; i++)
            {
                m_items[i].parent = this;
                m_items[i].prop = m_prop.GetArrayElementAtIndex(i);
            }
        }

        #endregion

        private class Item {
            public int id;
            public Rect rect;
            public DynamicList4 parent { get; set; }
            public SerializedProperty prop { get; set; }
            public float headerHeight { get; set; }

            private Rect m_myRect;
            private Rect m_headerRect;
            private Rect m_bodyRect;

            public Item() {
                headerHeight = 17f;
            }

            public void Draw() {
                m_myRect = rect;
                m_headerRect = new Rect(m_myRect.x+2, m_myRect.y+2, m_myRect.width-4, headerHeight-4);
                m_bodyRect = new Rect(m_myRect.x+2, m_myRect.y + headerHeight + 2, m_myRect.width-4, m_myRect.height - headerHeight);

                GUI.Box(m_bodyRect, "", m_style.body);
                GUI.Box(m_headerRect, "", m_style.header);

                parent.drawerItemHeader.DrawItemHeader(m_headerRect,prop);
                if(prop.isExpanded)
                    parent.drawerItemBody.DrawItemBody(m_bodyRect, prop);

                if (parent.reorderable)
                    GUI.Box(new Rect(m_myRect.x + 5f, m_myRect.y + 5f, 12f, headerHeight), "", m_style.dragHandle);
                if (GUI.Button(new Rect(m_myRect.x + m_myRect.width - headerHeight - 5f, m_myRect.y, headerHeight, headerHeight), m_style.minusButton, m_style.normal))
                    parent._deleteItem(this);
            }

            public bool isHeaderContainPoint(Vector2 point) {
                var detectorArea = new Rect(m_myRect.x + 5f, m_myRect.y + 5f, m_myRect.width - headerHeight, headerHeight);
                return detectorArea.Contains(point);
            }
        }
        private class Style
        {
            public GUIStyle normal = new GUIStyle();
            public GUIStyle normalTextMiddle = new GUIStyle();
            public GUIStyle box = new GUIStyle(GUI.skin.box);
            public GUIStyle header2 = new GUIStyle(EditorStyles.toolbar);
            public GUIContent plusButton = UnityEditorRes.IconToolbarPlus;
            public GUIContent minusButton = UnityEditorRes.IconToolbarMinus;
            public GUIStyle dragHandle = UnityEditorSkin.RLdraggingHandle;
            public GUIStyle header = UnityEditorSkin.RLheaderBackground;
            public GUIStyle headerLabel = UnityEditorSkin.midBoldLabel;
            public GUIStyle body = UnityEditorSkin.RLboxBackground;
            public GUIStyle itemBody = new GUIStyle(GUI.skin.box);
            public Style()
            {
                headerLabel.alignment = TextAnchor.MiddleLeft;
                normalTextMiddle.alignment = TextAnchor.UpperCenter;
            }
        }
    }
}
