// Imoet Dynamic List
// Copyright Yusuf Sulaiman (C) 2019 <yusufxh@ymail.com>
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#region Interface
/// <summary>
/// Interface to draw custom List Header
/// </summary>
public interface IDynamicListHeaderDrawer
{
    float GetHeaderHeight(SerializedProperty property);
    void DrawHeader(Rect rect, SerializedProperty property);
}
/// <summary>
/// Interface to draw custom Item List Header
/// </summary>
public interface IDynamicListItemHeaderDrawer
{
    float GetItemHeaderHeight(SerializedProperty property);
    void DrawItemHeader(Rect rect, SerializedProperty property);
}
/// <summary>
/// Interface to draw custom Item List Body
/// </summary>
public interface IDynamicListItemBodyDrawer
{
    float GetItemBodyHeight(SerializedProperty property);
    void DrawItemBody(Rect rect, SerializedProperty property);
}

#endregion Interface

namespace Imoet.UnityEditor
{
    public class DynamicList : IDynamicListHeaderDrawer, IDynamicListItemBodyDrawer, IDynamicListItemHeaderDrawer
    {
        #region Public Property
        /// <summary>
        /// Set to change mode of List
        /// </summary>
        public bool reorderable { get; set; }

        /// <summary>
        /// Interface to make a custom <see cref="drawerHeader"/>
        /// </summary>
        public IDynamicListHeaderDrawer drawerHeader { get; set; }
        /// <summary>
        /// Interface to make a custom <see cref="drawerItemHeader"/>
        /// </summary>
        public IDynamicListItemHeaderDrawer drawerItemHeader { get; set; }
        /// <summary>
        /// Interface to make a custom <see cref="drawerItemBody"/>
        /// </summary>
        public IDynamicListItemBodyDrawer drawerItemBody { get; set; }
        #endregion

        #region Private Variable
        private readonly SerializedProperty m_prop;
        private static Style m_style = null;

        private List<Item> m_items = null;

        private Item m_selectedItem = null;
        private Item m_hoveredItem = null;
        private Rect m_selectionRect = default(Rect);

        private bool m_sealed = false;
        private bool m_dragged = false;
        private Vector2 m_lastMousePos = default(Vector2);

        private Rect m_headerRect = default(Rect);
        private Rect m_bodyRect = default(Rect);
        #endregion

        #region Default Event Drawer
        /// <summary>
        /// Default Event Draver (DrawHeader). Use this method to fallback into default layout
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="property"></param>
        public void DrawHeader(Rect rect, SerializedProperty property) {
            EditorGUI.LabelField(rect, property.displayName, m_style.headerLabel);
        }
        /// <summary>
        /// Default Event Drawer (DrawItemHeader). Use this method to fallback into default layout
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="property"></param>
        public void DrawItemHeader(Rect rect, SerializedProperty property)
        {
            if (reorderable)
                GUI.Label(rect, property.displayName, m_style.normalTextMiddle);
            else {
                if (GUI.Button(rect, property.displayName, m_style.normalTextMiddle))
                    property.isExpanded = !property.isExpanded;
            }   
        }
        /// <summary>
        /// Default Event Draver (DrawItemBody). Use this method to fallback into default layout
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="property"></param>
        public void DrawItemBody(Rect rect, SerializedProperty property)
        {
            int inspectedDepth = property.depth;
            float lastHeight = 5f;
            SerializedProperty sProp = property.Copy();
            SerializedProperty eProp = property.GetEndProperty();
            foreach (SerializedProperty p in sProp)
            {
                if (SerializedProperty.EqualContents(p, eProp))
                {
                    break;
                }

                if (p.depth > inspectedDepth && p.depth <= inspectedDepth + 1)
                {
                    float propH = EditorGUI.GetPropertyHeight(p, null, true);
                    Rect r = new Rect(rect.x + 15, rect.y + lastHeight, rect.width - 25, propH);
                    EditorGUI.PropertyField(r, p, true);
                    lastHeight += propH + 2;
                }
            }
        }
        /// <summary>
        /// Default Event Draver (GetHeaderHeight). Use this method to fallback into default layout 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public float GetHeaderHeight(SerializedProperty property)
        {
            return 17f;
        }
        /// <summary>
        /// Default Event Draver (GetItemHeaderHeight). Use this method to fallback into default layout
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public float GetItemHeaderHeight(SerializedProperty property)
        {
            return 17f;
        }
        /// <summary>
        /// Default Event Draver (GetItemBodyHeight). Use this method to fallback into default layout
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public float GetItemBodyHeight(SerializedProperty property)
        {
            if (property.isExpanded && m_prop.arraySize > 0) {
                return EditorGUI.GetPropertyHeight(property, new GUIContent(property.displayName), true);
            }
            return 0;
        }

        #endregion Default Event Drawer

        #region Constructor
        /// <summary>
        /// Create a new <see cref="DynamicList"/> to rendering a list
        /// </summary>
        /// <param name="prop">Property to render. this property must be array</param>
        public DynamicList(SerializedProperty prop, bool reorderable = false)
        {
            if (prop == null) {
                throw new NullReferenceException("Property is Null");
            }

            if (!prop.isArray) {
                throw new ArgumentException("Property is not Array");
            }

            m_prop = prop;

            drawerHeader = this;
            drawerItemBody = this;
            drawerItemHeader = this;

            this.reorderable = reorderable;

            m_items = new List<Item>();
        }
        #endregion

        #region Public Function
        /// <summary>
        /// Add a new element into list
        /// </summary>
        public void Add()
        {
            m_prop.InsertArrayElementAtIndex(m_prop.arraySize);
            var nItem = new Item() {
                prop = m_prop.GetArrayElementAtIndex(m_prop.arraySize - 1),
                parent = this
            };
            nItem.isExpanded = !reorderable;
            m_items.Add(nItem);
        }
        /// <summary>
        /// Delete specific element of list
        /// </summary>
        /// <param name="idx">element's index that you want to remove</param>
        public void Delete(int idx)
        {
            m_prop.DeleteArrayElementAtIndex(idx);
            m_items.RemoveAt(idx);
            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].prop = m_prop.GetArrayElementAtIndex(i);
            }
        }
        /// <summary>
        /// Move / Swap position of element of list
        /// </summary>
        /// <param name="src">source index of element</param>
        /// <param name="dst">destination index of element</param>
        public void Move(int src, int dst)
        {
            m_prop.MoveArrayElement(m_selectedItem.id, dst);
            m_items.RemoveAt(m_selectedItem.id);
            m_items.Insert(dst, m_selectedItem);
            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].prop = m_prop.GetArrayElementAtIndex(i);
                m_items[i].prop.isExpanded = m_items[i].isExpanded;
            }
        }
        /// <summary>
        /// Draw List into GUI. Everything is already managed so you just to call this method after initialization
        /// </summary>
        public void Draw()
        {
            //Check if property is null
            if (m_prop == null)
            {
                Debug.LogError("Trying to draw nulled property, this is not allowed");
                return;
            }
            m_prop.isExpanded = false;
            //Style
            if (m_style == null)
            {
                m_style = new Style();
            }

            //If Array Suddenly Changed, Remake
            if (m_items != null && m_items.Count != m_prop.arraySize)
            {
                var nSize = m_prop.arraySize;
                m_items = new List<Item>(nSize);
                for (int i = 0; i < nSize; i++)
                {
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
            var hRect = new Rect(m_headerRect.x, m_headerRect.y + 2.5f, m_headerRect.width, m_headerRect.height - 5f);
            GUI.Box(hRect, "", m_style.header2);
            var drawHeader = new Rect(hRect.x + 7f, hRect.y, hRect.width, hRect.height);
            drawerHeader.DrawHeader(drawHeader, m_prop);
            if (GUI.Button(new Rect(hRect.width - 5f, hRect.y, hRect.height, hRect.height), m_style.plusButton, m_style.normal)) {
                Add();
            }

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
            else
            {
                //Draw Empty
                var emptyRect = GUILayoutUtility.GetRect(0, 22f);
                GUI.Box(emptyRect, "", m_style.body);
                var labelRect = new Rect(emptyRect.x + 4f, emptyRect.y + 2f, emptyRect.width-8f, emptyRect.height - 4f);
                EditorGUI.LabelField(labelRect, "List is Empty");
            }
            EditorGUILayout.Space();
        }

        #endregion Public Function

        #region Private Function
        private void _drawOnEvent(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (!m_sealed && reorderable)
                    {
                        for (int i = 0; i < m_items.Count; i++)
                        {
                            if (m_items[i].isHeaderContainPoint(e.mousePosition) && m_selectedItem == null)
                            {
                                m_selectedItem = m_items[i];
                                m_selectionRect = default(Rect);

                                m_lastMousePos = e.mousePosition;
                                m_sealed = true;

                                e.Use();
                            }
                            m_items[i].id = i;
                            m_items[i].isExpanded = m_items[i].prop.isExpanded;
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (m_sealed && m_selectedItem != null)
                    {
                        m_dragged = true;
                        var mousePos = e.mousePosition;
                        m_hoveredItem = _findNearestFromPoint(mousePos);
                        var mouseDir =  Mathf.Sign(e.delta.y);
                        if (m_hoveredItem != null && m_hoveredItem != m_selectedItem) {
                            var nearestItemRect = m_hoveredItem.rect;
                            var center = nearestItemRect.center;
                            bool cantMove = (m_hoveredItem.id + mouseDir) == m_selectedItem.id;
                            if (!cantMove)
                            {
                                if (mouseDir < 0 && mousePos.y < center.y)
                                {
                                    m_selectionRect = new Rect(nearestItemRect.x, nearestItemRect.y - 2, nearestItemRect.width, 5);
                                }
                                else if (mouseDir > 0 && mousePos.y > center.y)
                                {
                                    m_selectionRect = new Rect(nearestItemRect.x, nearestItemRect.y + nearestItemRect.height, nearestItemRect.width, 5);
                                }
                            }
                            else {
                                m_selectionRect = new Rect();
                            }
                        }
                        e.Use();
                    }
                    break;
                case EventType.Ignore:
                case EventType.MouseUp:
                    if (m_sealed)
                    {
                        var mousePos = e.mousePosition;
                        //Detect "Click"
                        if (!m_dragged)
                        {
                            var pMouseDist = Vector2.Distance(mousePos, m_lastMousePos);
                            if (pMouseDist < 0.1f) {
                                m_selectedItem.prop.isExpanded = !m_selectedItem.prop.isExpanded;
                            }
                        }

                        //Detect Movement
                        else {
                            var hoveredItemCenter = m_hoveredItem.rect.center;
                            var tgtID = m_hoveredItem.id;
                            if (mousePos.y > hoveredItemCenter.y && m_selectedItem.id > m_hoveredItem.id)
                                tgtID = m_hoveredItem.id + 1;
                            else if (mousePos.y < hoveredItemCenter.y && m_selectedItem.id < m_hoveredItem.id)
                                tgtID = m_hoveredItem.id - 1;
                            tgtID = Mathf.Clamp(tgtID, 0, m_items.Count - 1);
                            Move(m_selectedItem.id, tgtID);
                        }

                        m_dragged = false;
                        m_selectedItem = null;
                        m_hoveredItem = null;
                        m_sealed = false;
                        e.Use();
                    }
                    break;
            }
        }

        private Item _findNearestFromPoint(Vector2 point)
        {
            var nearest = float.MaxValue;
            Item res = null;
            for (int i = 0; i < m_items.Count; i++) {
                var itemCenter = m_items[i].rect.center;
                var dist = Vector2.Distance(point, itemCenter);
                if (dist < nearest) {
                    nearest = dist;
                    res = m_items[i];
                }
            }
            return res;
        }

        private void _drawItems(Rect body)
        {
            if (m_sealed) {
                for (int i = 0; i < m_items.Count; i++) {
                    var item = m_items[i];

                    if (item == m_selectedItem){
                        continue;
                    }

                    item.Draw();
                }

                if (m_sealed) {
                    EditorGUI.BeginDisabledGroup(true);
                    m_selectedItem.Draw();
                    EditorGUI.EndDisabledGroup();
                    if(m_dragged)
                        GUI.Box(m_selectionRect, "", m_style.selectionBox);
                }
            }
            else
            {
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

        private Rect _getBodyRect()
        {
            var res = 0.0f;
            for (int i = 0; i < m_items.Count; i++) {
                res += drawerItemHeader.GetItemHeaderHeight(m_items[i].prop) + drawerItemBody.GetItemBodyHeight(m_items[i].prop) + 2f;
            }

            return GUILayoutUtility.GetRect(0, res + 6f);
        }

        private void _deleteItem(Item item)
        {
            int idx = m_items.IndexOf(item);
            m_prop.DeleteArrayElementAtIndex(idx);
            m_prop.serializedObject.ApplyModifiedProperties();
            m_items.RemoveAt(idx);
            for (int i = idx; i < m_items.Count; i++) {
                m_items[i].parent = this;
                m_items[i].prop = m_prop.GetArrayElementAtIndex(i);
            }
        }

        #endregion Private Function

        #region Klass
        private class Item
        {
            public int id;
            public Rect rect;
            public DynamicList parent { get; set; }
            public SerializedProperty prop {
                get { return m_prop; }
                set {
                    if (parent != null && !parent.m_sealed) {
                        if (value == null)
                            m_propExpanded = false;
                        else
                            m_propExpanded = value.isExpanded;
                    }
                    m_prop = value;
                }
            }
            public float headerHeight { get; set; }
            public bool isExpanded {
                get { return m_propExpanded; }
                set {
                    m_propExpanded = value;
                    prop.isExpanded = value;
                }
            }
            
            internal SerializedProperty m_prop;
            internal bool m_propExpanded;
            internal Rect m_myRect;
            internal Rect m_headerRect;
            internal Rect m_bodyRect;

            public Item(){
                headerHeight = 20f;
            }

            public void Draw()
            {
                m_myRect = rect;
                m_headerRect = new Rect(m_myRect.x + 2, m_myRect.y + 2, m_myRect.width - 4, headerHeight - 4);
                m_bodyRect = new Rect(m_myRect.x + 2, m_myRect.y + headerHeight + 2, m_myRect.width - 4, m_myRect.height - headerHeight);

                if (prop.isExpanded)
                    GUI.Box(m_bodyRect, "", m_style.body);
                GUI.Box(m_headerRect, "", m_style.header);
                m_headerRect.x += 14;
                m_headerRect.height += 4;
                m_headerRect.width -= headerHeight + 20f;
                parent.drawerItemHeader.DrawItemHeader(m_headerRect, prop);
                if (parent.reorderable)
                {
                    GUI.Box(new Rect(m_myRect.x + 5f, m_myRect.y + 8f, 12f, headerHeight - 3f), "", m_style.dragHandle);
                }
                if (GUI.Button(new Rect(m_myRect.x + m_myRect.width - headerHeight - 5f, m_myRect.y, headerHeight, headerHeight), m_style.minusButton, m_style.normal))
                {
                    parent._deleteItem(this);
                    return;
                }
                if (prop.isExpanded){
                    parent.drawerItemBody.DrawItemBody(new Rect(m_bodyRect.x + 4f, m_bodyRect.y + 2f, m_bodyRect.width-8f, m_bodyRect.height-4f), prop);
                }
            }

            public bool isHeaderContainPoint(Vector2 point)
            {
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
            public GUIStyle selectionBox = new GUIStyle(GUI.skin.button);

            public Style()
            {
                selectionBox.normal.background = selectionBox.focused.background;
                headerLabel.alignment = TextAnchor.MiddleLeft;
                normalTextMiddle.alignment = TextAnchor.MiddleCenter;
            }
        }
        #endregion
    }
}