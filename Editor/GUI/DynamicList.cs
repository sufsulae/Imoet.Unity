using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Imoet.Unity.Animation;
using U = UnityEngine;
namespace Imoet.UnityEditor{
	public class DynamicList {
		private List<Item> m_items = new List<Item>();
		private SerializedProperty m_inspectedProp;
		private int m_arrayCount;
		private bool m_dragged;
		private static Style m_style;
		
		//Config
		private float m_headerHeight = 17;
		private float m_itemHeaderHeight = 17;
		private bool m_itemHasBody = true;
		private float m_itemBodySize = 32;
		private float m_animateMoveSpeed = 0.5f;
		private bool m_orderable;
		private float m_allItemHeight;
		private U.Rect m_itemAreaRect;
		
		//Temp
		private U.Vector2 m_startMousePos;
		private Item m_selectedItem;
		private int m_selectedItemIdx;
		private U.Rect m_selectedItemRect;
		private U.Vector2 m_lastDelta;
		private Action m_repaint;
		private bool m_reqRepaint;
		
		private bool[] m_tmpBool;
		
		//Property
		public float headerHeight{
			get{return m_headerHeight;}
			set{m_headerHeight = value;}
		}
		public float itemHeaderHeight{
			get{return m_itemHeaderHeight;}
			set{m_itemHeaderHeight = value;}
		}
		public float itemBodySize{
			get{return m_itemBodySize;}
			set{m_itemBodySize = value;}
		}
		public bool itemHasBody{
			get{return m_itemHasBody;}
			set{m_itemHasBody = value;}
		}
		public float animSpeed{
			get{return m_animateMoveSpeed;}
			set{m_animateMoveSpeed = value;}
		}
		public Action<U.Rect,SerializedProperty> headerDrawCallBack{ get; set; }
		public Action<U.Rect,SerializedProperty> itemHeaderDrawCallBack{ get; set; }
		public Action<U.Rect,SerializedProperty> itemBodyDrawCallBack{ get; set; }
		
		//Construtor
		public DynamicList(){}
		public DynamicList(SerializedProperty property, bool orderable, Action repaint){
			if(property == null)
				throw new UnityException("Property is Null!!!");
			if(!property.isArray)
				throw new UnityException("Only accepted array property");
			m_inspectedProp = property;
			m_orderable = orderable;
			m_repaint = repaint;
		}
		public DynamicList(SerializedProperty property, bool orderable) : this(property,orderable,null){}
		public DynamicList(SerializedProperty property) : this(property,true){}
		
		//Private Function
		private Item FindItem(int id)
		{
			for(int i=0; i<m_arrayCount; i++)
			if(m_items[i].id == id){
				return m_items[i];
			}
			return null;
		}
		
		private void ValidateEvent(Event e){
			switch(e.type){
			case EventType.mouseDown:
				for(int i=0; i<m_arrayCount; i++){
					//If We Found any rect that its handle contain the mousePosition, select it
					if(m_items[i].dragHandleRect.Contains(e.mousePosition)){
						m_selectedItem = m_items[i];
						m_selectedItemRect = m_selectedItem.rect;
						m_selectedItemIdx = i;
						m_selectedItem.selected = true;
						m_tmpBool = new bool[m_arrayCount];
						for(int x=0; x<m_arrayCount; x++){
							m_tmpBool[x] = m_items[x].expanded;
						}
						break;
					}
				}
				if(m_selectedItem != null){
					//Mark our mouse position for calculating delta position
					m_startMousePos = e.mousePosition;
					
					//Calculate Item Area Size for Drawing
					m_itemAreaRect = m_items[0].rect;
					m_itemAreaRect.height = 0;
					for(int i=0; i<m_arrayCount; i++){
						//Lock All Item so every item has rect from last information
						m_items[i].locked = true;
						m_itemAreaRect.height += m_items[i].rect.height+4;
					}
					e.Use();
				}
				break;
			case EventType.mouseDrag:
				if(m_orderable && m_selectedItem != null){
					m_dragged = true;
					U.Vector2 mousePosGap = e.mousePosition - m_startMousePos;
					U.Vector2 deltaMousePosition = mousePosGap - m_lastDelta;
					m_lastDelta = mousePosGap;
					
					float selectedPos = m_selectedItemRect.y+mousePosGap.y;
					if(selectedPos < m_itemAreaRect.yMin)
						selectedPos = m_itemAreaRect.yMin;
					else if(selectedPos + m_selectedItem.rect.height > m_itemAreaRect.yMax)
						selectedPos = m_itemAreaRect.yMax - m_selectedItem.rect.height;
					m_selectedItem.rect = new U.Rect(m_selectedItemRect.x,selectedPos,m_selectedItemRect.width,m_selectedItemRect.height);
					
					if(deltaMousePosition.y != 0){
						U.Rect selectedRect = m_selectedItem.rect;
						U.Rect tmpRect = new U.Rect();
						int deltaID = 0;
						for(int i=0; i<m_arrayCount; i++){
							U.Rect iRect = m_items[i].rect;
							if(m_items[i] != m_selectedItem)
							{
								if(selectedRect.Contains(iRect.center)) {
									deltaID = m_items[i].id - m_selectedItem.id;
								}
							}
						}
						
						if(deltaID != 0){
							int dir = (int)Mathf.Sign(deltaID);
							deltaID = (deltaID < 0) ? -deltaID : deltaID;
							while(deltaID > 0){
								Item nextItem = FindItem(m_selectedItem.id + dir);
								
								tmpRect = nextItem.rect;
								tmpRect.y -= (m_selectedItem.rect.height + 4) * dir;
								nextItem.rect = tmpRect;
								
								bool tmp = m_tmpBool[m_selectedItem.id];
								m_tmpBool[m_selectedItem.id] = m_tmpBool[m_selectedItem.id + dir];
								m_tmpBool[m_selectedItem.id + dir] = tmp;
								
								nextItem.id -= dir;
								m_selectedItem.id += dir;
								
								deltaID--;
							}
						}
					}
					e.Use();
				}
				break;
			case EventType.ignore:
			case EventType.mouseUp:
				if(m_selectedItem != null)
				{
					//Open All locked Rect
					for(int i=0; i<m_arrayCount; i++){
						m_items[i].expanded = m_tmpBool[i];
						m_items[i].locked = false;
					}
					
					//If we select the item and release without making any movement,We can assume we just click that item
					if(!m_dragged && m_startMousePos == e.mousePosition)
						m_selectedItem.expanded = !m_selectedItem.expanded;
					
					//If We move the item, check if it is moving or not
					else if(m_selectedItemIdx != m_items[m_selectedItemIdx].id && m_orderable){
						m_inspectedProp.MoveArrayElement(m_selectedItemIdx,m_selectedItem.id);
						for(int i=0; i<m_arrayCount; i++) {
							m_items[i].property = m_inspectedProp.GetArrayElementAtIndex(i);
						}
					}
					//Reset Value
					for(int i=0; i<m_arrayCount; i++){
						m_items[i].id = i;
					}
					m_selectedItem.selected = false;
					m_selectedItem = null;
					m_selectedItemIdx = -1;
					m_dragged = false;
					m_lastDelta = U.Vector2.zero;
					e.Use();
				}
				break;
			}
		}
		
		//Public Function
		public void Draw(){
			//Check Style if Dissapear
			if (m_style == null)
				m_style = new Style ();
			
			//Refresh All Items if Array Suddenly Changed
			if(m_arrayCount != m_inspectedProp.arraySize)
				RefreshAllItem();
			
			#region DRAW
			EditorGUILayout.BeginVertical(m_style.baseBackground);
			
			#region Header
			if(headerDrawCallBack != null){
				U.Rect headerRect = GUILayoutUtility.GetRect(0,m_headerHeight);
				headerDrawCallBack(headerRect,m_inspectedProp);
			}
			else{
				EditorGUILayout.LabelField(m_inspectedProp.displayName,m_style.headerLeft);
				U.Rect buttonRect = GUILayoutUtility.GetLastRect();
				if(GUI.Button(new U.Rect(buttonRect.width + 5,buttonRect.y,15,15),m_style.plusButton,m_style.normal))
					AddItem();
			}
			#endregion
			
			#region ItemArea
			if(m_arrayCount > 0){
				EditorGUILayout.BeginVertical(m_style.itemAreaBackground);
				
				for(int i=0; i<m_arrayCount; i++){
					if(m_items[i] != m_selectedItem){
						if(!m_items[i].locked)
							m_items[i].id = i;
						m_items[i].Draw();
					}
				}
				
				//Debug.Log ("Has Repaint " + (m_repaint != null));
				if(m_reqRepaint)
				{
					m_reqRepaint = false;
					if(m_repaint != null)
						m_repaint.Invoke();
				}
				
				//Validate Event
				ValidateEvent(Event.current);
				
				//Draw Selected Item
				if(m_selectedItem != null)
				{
					GUILayoutUtility.GetRect(0,m_itemAreaRect.height);
					m_selectedItem.Draw();
				}
				EditorGUILayout.EndVertical();
			}
			#endregion
			
			EditorGUILayout.EndVertical();
			
			#endregion
		}
		
		public void AddItem(){
			Item i = new Item();
			if(m_inspectedProp != null)
			{
				m_inspectedProp.InsertArrayElementAtIndex(m_arrayCount);
				i.id = m_arrayCount;
				i.parent = this;
				i.Setup(m_inspectedProp.GetArrayElementAtIndex(m_arrayCount));
				i.expanded = false;
			}
			m_items.Add(i);
			m_arrayCount++;
		}
		
		public void DeleteItem(int idx){
			m_items.RemoveAt(idx);
			if(m_inspectedProp != null)
				m_inspectedProp.DeleteArrayElementAtIndex(idx);
			m_arrayCount--;
			if(m_inspectedProp != null){
				for(int i=idx; i<m_arrayCount; i++){
					m_items[i].id = i;
					m_items[i].parent = this;
					m_items[i].property = m_inspectedProp.GetArrayElementAtIndex(i);
				}
			}
		}
		
		public void DeleteItem(Item item){
			DeleteItem(m_items.IndexOf(item));
		}
		
		public void ClearItem(){
			if(m_inspectedProp != null)
				m_inspectedProp.ClearArray();
			m_items.Clear();
			m_arrayCount = 0;
		}
		
		public void RefreshAllItem(){
			m_arrayCount = m_inspectedProp.arraySize;
			m_items = new List<Item>();
			for(int i=0; i<m_arrayCount; i++)
			{
				Item newItem = new Item();
				newItem.id = i;
				newItem.parent = this;
				newItem.Setup(m_inspectedProp.GetArrayElementAtIndex(i));
				m_items.Add(newItem);
			}
		}

		/*
		private void PrintAllItemID(){
			string debugString = "";
			for(int i=0; i<m_arrayCount; i++){
				debugString += m_items[i].id.ToString();
			}
			Debug.Log(debugString);
		}
		
		private void PrintAllItemExpanded(){
			string debugString = "";
			for(int i=0; i<m_arrayCount; i++){
				debugString += m_items[i].expanded + ",";
			}
			Debug.Log(debugString);
		}
		
		private void PrintAllBool(IEnumerable<bool> list){
			string debugString = ""; 
			foreach(bool b in list){
				debugString += b + ",";
			}
			Debug.Log(debugString);
		}
		*/
		
		#region Internal Class
		public class Item{
			internal DynamicList parent;
			//Property
			internal int id{
				get{return m_id;}
				set{m_id = value;}
			}
			internal U.Rect rect{
				get{return m_myRect;}
				set{
					m_myRect = value;
				}
			}
			internal SerializedProperty property{
				get{
					return m_prop;
				}
				set{
					if(value != m_prop){
						if(m_prop == null)
							m_expanded = value.isExpanded;
						else if(m_prop != value)
						{
							value.isExpanded = m_expanded;
						}
						m_prop = value;
					}
				}
			}
			internal U.Rect dragHandleRect{
				get{
					return m_handleRect;
				}
			}
			internal bool expanded{
				get{
					return m_expanded;
				}
				set{
					m_expanded = value;
					if(m_prop != null && !m_isLocked)
						m_prop.isExpanded = m_expanded;
				}
			}
			internal bool hasBody{
				get{return m_hasBody;}
				set{m_hasBody = value;}
			}
			internal bool locked{
				get{return m_isLocked;}
				set{m_isLocked = value;}
			}
			internal bool selected{
				get{return m_selected;}
				set{m_selected = value;}
			}
			
			//Private Field
			private int m_id;
			private U.Rect m_myRect,m_handleRect;
			private U.Rect m_drawRect;
			private SerializedProperty m_prop;
			private bool m_expanded,m_hasBody,m_isLocked,m_selected;
			//private UTweener<U.Rect> tweener;
			
			public void Setup(SerializedProperty prop){
				m_prop = prop;
				m_expanded = prop.isExpanded;
			}
			
			public void Draw(){
				//Calculate Item Area
				if(!m_isLocked)
				{
					if(parent.itemHeaderDrawCallBack != null){
						m_myRect.height = parent.itemHeaderHeight;
					}
					if(parent.itemBodyDrawCallBack != null && parent.m_itemHasBody)
						m_myRect.height += parent.m_itemBodySize;
					if(parent.itemHeaderDrawCallBack == null && parent.itemBodyDrawCallBack == null && m_prop != null){
						if(parent.m_itemHasBody)
							m_myRect.height = EditorGUI.GetPropertyHeight(m_prop,null,true);
						else
							m_myRect.height = 17;
					}
					m_myRect = GUILayoutUtility.GetRect(0,m_myRect.height+4);
					m_myRect.y += 2;
					m_myRect.height -= 4;
					m_drawRect = m_myRect;
				}
				
				//Check State
				if(m_selected){
					m_drawRect = m_myRect;
				}
				else{
					if(m_drawRect != m_myRect){
						if(parent.m_repaint != null){
							if(!parent.m_reqRepaint)
								parent.m_reqRepaint = true;
                            //							if(Event.current.type == EventType.Repaint)
                            //m_drawRect = TweenWrap.Tween(m_drawRect,m_myRect,0.05f,Imoet.Animation.TweenType.Linear);
                            m_drawRect = m_myRect;
						}
						else{
							m_drawRect = m_myRect;
						}
					}
				}
				#region Body
				if(parent.itemBodyDrawCallBack != null && parent.m_itemHasBody){
					parent.itemBodyDrawCallBack(new U.Rect(m_drawRect.x,m_drawRect.y+17,m_drawRect.width,parent.m_itemHeaderHeight-17),m_prop);
				}
				else{
					if(m_prop != null && m_prop.isExpanded){
						EditorGUILayoutX.BeginWideGUI();
						U.Rect itemRect = new U.Rect(m_drawRect.x,m_drawRect.y+17,m_drawRect.width,m_drawRect.height-17);
						GUI.Box(m_drawRect,"",m_style.itemBodyBackground);
						int depth = m_prop.depth+1;
						int itemCount = -1;
						float lastH = 0;
						SerializedProperty sProp = m_prop.Copy();
						SerializedProperty eProp = sProp.GetEndProperty();
						foreach(SerializedProperty p in sProp)
						{
							if(SerializedProperty.EqualContents(p,eProp))
								break;
							if(p.depth > depth-1 && p.depth <=depth)
							{
								float propH = EditorGUI.GetPropertyHeight(p,null,true);
								U.Rect r = new U.Rect(itemRect.x + 15, itemRect.y+lastH,itemRect.width-25,propH);
								EditorGUI.PropertyField(r,p,true);
								itemCount++;
								lastH+=propH+1;
							}
						}
						sProp.Dispose();
						eProp.Dispose();
						EditorGUILayoutX.EndWideGUI();
					}
				}
				#endregion
				
				#region Header
				GUI.Box(m_drawRect,"",m_style.itemHeaderBackground);
				U.Rect headerRect = new U.Rect(m_drawRect.x,m_drawRect.y,m_drawRect.width,17);
				if(parent.itemHeaderDrawCallBack != null){
					headerRect = new U.Rect(m_drawRect.x,m_drawRect.y,m_drawRect.width,parent.m_itemHeaderHeight);
					if(parent.m_orderable){
						headerRect.x += headerRect.height;
						headerRect.width -= headerRect.height*2;
					}
					parent.itemHeaderDrawCallBack(headerRect,m_prop);
				}
				else{
					if(m_prop != null)
						GUI.Label(new U.Rect(headerRect.x + headerRect.height + headerRect.height/4,headerRect.y,headerRect.width-headerRect.height*2,headerRect.height),m_prop.displayName,m_style.header);
				}
				if(parent.m_orderable)
					GUI.Box(new U.Rect(headerRect.x+headerRect.height/4,headerRect.y+headerRect.height/3.5f,headerRect.height,headerRect.height),"",m_style.itemHandle);
				if(GUI.Button(new U.Rect(headerRect.x + headerRect.width-headerRect.height,headerRect.y,headerRect.height,headerRect.height),"",m_style.xButton)){
					parent.DeleteItem(this);
					return;
				}
				m_handleRect = headerRect;
				#endregion
			}
		}
		
		private class Style
		{
			public GUIStyle headerLeft;
			public GUIStyle normal = new GUIStyle();
			public GUIStyle button = new GUIStyle(GUI.skin.button);
			public GUIStyle baseBackground = new GUIStyle(GUI.skin.button);
			public GUIStyle itemAreaBackground = EditorStyles.textArea;
			public GUIStyle itemBodyBackground = UnityEditorSkin.RLboxBackground;
			public GUIStyle itemHeaderBackground = UnityEditorSkin.RLheaderBackground;
			public GUIStyle header = UnityEditorSkin.midBoldLabel;
			public GUIStyle itemHandle = UnityEditorSkin.windowBottomResize;
			public Texture plusButton = UnityEditorRes.IconToolbarPlus.image;
			public GUIStyle xButton = UnityEditorSkin.windowCloseButton;
			public Style()
			{
				headerLeft = new GUIStyle(header);
				headerLeft.alignment = TextAnchor.UpperLeft;
			}
		}
		#endregion
	}
}