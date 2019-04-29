using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
namespace Imoet.UnityEditor{
	public class DynamicList2 : IDynamicListHeaderDrawer,IDynamicListItemBodyDrawer,IDynamicListItemHeaderDrawer {
		#pragma warning disable 0649
		//Property
		public Action repaint{
			get{return m_repaint;}
			set{m_repaint = value;}
		}
		public IDynamicListHeaderDrawer onDrawHeader{
			get{return m_headerDrawer;}
			set{
				if(value == null)
					m_headerDrawer = this;
				else
					m_headerDrawer = value;
			}
		}
		public IDynamicListItemHeaderDrawer onDrawItemHeader{
			get{return m_itemHeaderDrawer;}
			set{
				if(value == null)
					m_itemHeaderDrawer = this;
				else
					m_itemHeaderDrawer = value;
			}
		}
		public IDynamicListItemBodyDrawer onDrawItemBody{
			get{return m_itemBodyDrawer;}
			set{
				if(value == null)
					m_itemBodyDrawer = this;
				else
					m_itemBodyDrawer = value;
			}
		}

		//Private Field
		private List<Item> m_items;
		private SerializedProperty m_prop;
		private bool m_orderable;
		private Action m_repaint;
		private IDynamicListHeaderDrawer m_headerDrawer;
		private IDynamicListItemBodyDrawer m_itemBodyDrawer;
		private IDynamicListItemHeaderDrawer m_itemHeaderDrawer;
		private int m_itemCount;
		private bool m_locked;
        private Item m_selectedItem;

		//Private static Field
		private static Style m_style;
		//Constructor
		public DynamicList2(SerializedProperty property) : this(property,true){}
		public DynamicList2(SerializedProperty property,bool orderable){
			if(property == null)
				throw new UnityException("Property is Null!!!");
			if(!property.isArray)
				throw new UnityException("Only accepted array property");
			m_headerDrawer = this; //This is Default Setting
			m_itemBodyDrawer = this; //This is Default Setting
			m_itemHeaderDrawer = this; //This is Default Setting
			m_items = new List<Item>();
			m_prop = property;
			m_orderable = orderable;
		}

		//Public Draw
		public void Draw(){
			//Check Style 
			if(m_style == null)
				m_style = new Style();

			//Remake List if array suddenly changed
			if(m_itemCount != m_prop.arraySize){
				m_items = new List<Item>();
				m_itemCount = m_prop.arraySize;
				for(int i=0; i<m_itemCount; i++){
					m_items.Add(new Item(m_prop.GetArrayElementAtIndex(i)){parent = this});
				}
			}

            //Check Event
            var e = Event.current;

			//Draw Header
			Rect headerRect = GUILayoutUtility.GetRect(0,m_headerDrawer.GetHeaderHeight(m_prop)+5);
			headerRect.y += 2.5f;
			headerRect.height -=5f;
			GUI.Box(headerRect,"",m_style.header2);
			m_headerDrawer.DrawHeader(new Rect(headerRect.x,headerRect.y,headerRect.width-headerRect.height,headerRect.height),m_prop);
			if(GUI.Button (new Rect(headerRect.width-5f,headerRect.y,headerRect.height,headerRect.height),m_style.plusButton,m_style.normal)){
				Add();
			}
			//Draw Body
			if(m_itemCount > 0){
				//Draw Background
				float totalItemHeight = 0;
				for(int i=0; i<m_itemCount; i++){
					totalItemHeight += m_itemHeaderDrawer.GetItemHeaderHeight(m_items[i].inspectedProperty) + m_itemBodyDrawer.GetItemBodyHeight(m_items[i].inspectedProperty) + 2f;
				}
				Rect bodyRect = GUILayoutUtility.GetRect(0,totalItemHeight+10f);
				GUI.Box(bodyRect,"",m_style.body);

				//Setup Item
				Rect itemRect = new Rect(bodyRect.x + 5f, bodyRect.y + 3f, bodyRect.width-10f, bodyRect.height);
				Rect tempRect = itemRect;
				float lastHeight = 0;
				for(int i=0; i<m_itemCount; i++){

					Item item = m_items[i];
                    float itemHeaderHeight = m_itemHeaderDrawer.GetItemHeaderHeight(m_items[i].inspectedProperty);
                    float itemBodyHeight = m_itemBodyDrawer.GetItemBodyHeight(m_items[i].inspectedProperty);
                    if (i == 0)
                    {
                        item.headerRect = new Rect(tempRect.x, tempRect.y, tempRect.width, itemHeaderHeight);
                        item.bodyRect = new Rect(tempRect.x, tempRect.y + itemHeaderHeight, tempRect.width, itemBodyHeight);
                    }
                    else
                    {
                        item.headerRect = new Rect(tempRect.x, tempRect.y + lastHeight, tempRect.width, itemHeaderHeight);
                        item.bodyRect = new Rect(tempRect.x, tempRect.y + lastHeight + itemHeaderHeight, tempRect.width, itemBodyHeight);
                    }
					lastHeight += itemHeaderHeight + itemBodyHeight + 2f;
				}

                //Apply Control And Draw
                for (int i = 0; i < m_itemCount; i++){
                    Item item = m_items[i];
                    item.Draw();
                }
			}
		}
		public void Add(){
			m_prop.InsertArrayElementAtIndex(m_prop.arraySize);
			m_items.Add(new Item(m_prop.GetArrayElementAtIndex(m_prop.arraySize-1)){parent = this});
			m_itemCount++;
			if(m_repaint != null)
				m_repaint();
		}
		public void Delete(int idx){
			m_prop.DeleteArrayElementAtIndex(idx);
			m_items.RemoveAt(idx);
			m_itemCount--;
			if(m_repaint != null)
				m_repaint();
		}
		private void Delete(Item item){
			int idx = m_items.IndexOf(item);
			m_prop.DeleteArrayElementAtIndex(idx);
			m_items.Remove(item);
			m_itemCount--;
			for(int i=idx; i<m_itemCount; i++){
				m_items[i].parent = this;
				m_items[i].inspectedProperty = m_prop.GetArrayElementAtIndex(i);
			}
			if(m_repaint != null)
				m_repaint();
		}
		public void RefreshAllItem(){
			m_itemCount = m_prop.arraySize;
			m_items = new List<Item>();
			for(int i=0; i<m_itemCount; i++)
			{
				Item newItem = new Item(m_prop.GetArrayElementAtIndex(i));
				newItem.parent = this;
				newItem.isExpanded = newItem.inspectedProperty.isExpanded;
				m_items.Add(newItem);
			}
		}

		//Listener
		public virtual void DrawHeader(Rect rect, SerializedProperty property){
			rect.x += 7f;
			EditorGUI.LabelField(rect,property.displayName,m_style.headerLabel);
		}
		public virtual float GetHeaderHeight (SerializedProperty property){
			return 17f;
		}
		public virtual void DrawItemHeader(Rect rect, SerializedProperty property){
            GUI.Label(rect, property.displayName, m_style.normalTextMiddle);
		}
		public virtual float GetItemHeaderHeight(SerializedProperty property){
			return 17f;
		}
		public virtual float GetItemBodyHeight(SerializedProperty property){
			if(property.isExpanded)
				return EditorGUI.GetPropertyHeight(property,new GUIContent(property.displayName),true);
			return 0;
		}
		public virtual void DrawItemBody(Rect rect, SerializedProperty property){
			int inspectedDepth = property.depth;
			float lastHeight = 5f;
			SerializedProperty sProp = property.Copy();
			SerializedProperty eProp = property.GetEndProperty();
			foreach(SerializedProperty p in sProp){
				if(SerializedProperty.EqualContents(p,eProp))
					break;
				if(p.depth > inspectedDepth && p.depth <= inspectedDepth+1){
					float propH = EditorGUI.GetPropertyHeight(p,null,true);
					Rect r = new Rect(rect.x + 15, rect.y+lastHeight,rect.width-25,propH);
					EditorGUI.PropertyField(r,p,true);
					lastHeight+=propH+2;
				}
			}
		}

		//Private Class
		private class Item{
			public Item(SerializedProperty property){
				this.inspectedProperty = property;
			}
			public SerializedProperty inspectedProperty;
			public DynamicList2 parent;
			public bool isExpanded;
			public Rect myRect,headerRect,bodyRect;
			public void Draw(){
				if(!parent.m_locked)
					myRect = new Rect(headerRect.x,headerRect.y,headerRect.width,bodyRect.height+headerRect.height);
				Rect newHeaderRect = new Rect(myRect.x + 20f,myRect.y,myRect.width-40f,headerRect.height);
				Rect newBodyRect = new Rect(myRect.x,myRect.y + headerRect.height,myRect.width,bodyRect.height);
				GUI.Box(new Rect(myRect.x + 2,myRect.y+2,myRect.width-4,myRect.height-4),"",m_style.body);
				GUI.Box(new Rect(myRect.x, myRect.y,myRect.width,headerRect.height),"",m_style.header);
				parent.m_itemHeaderDrawer.DrawItemHeader(newHeaderRect,inspectedProperty);
				if(inspectedProperty.isExpanded){
					parent.m_itemBodyDrawer.DrawItemBody(newBodyRect,inspectedProperty);
				}
				if(parent.m_orderable)
					GUI.Box(new Rect(myRect.x + 5f,myRect.y + 5f,12f,headerRect.height),"",m_style.dragHandle);
				if(GUI.Button(new Rect(myRect.width,myRect.y,myRect.height,myRect.height),m_style.minusButton,m_style.normal))
					parent.Delete(this);
			}
		}

		private class Style{
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
			public Style(){
				headerLabel.alignment = TextAnchor.MiddleLeft;
				normalTextMiddle.alignment = TextAnchor.UpperCenter;
			}
		}
	}

	//Interface for Drawer
	public interface IDynamicListHeaderDrawer{
		float GetHeaderHeight(SerializedProperty property);
		void DrawHeader(Rect rect, SerializedProperty property);
	}
	public interface IDynamicListItemHeaderDrawer{
		float GetItemHeaderHeight(SerializedProperty property);
		void DrawItemHeader(Rect rect, SerializedProperty property);
	}
	public interface IDynamicListItemBodyDrawer{
		float GetItemBodyHeight(SerializedProperty property);
		void DrawItemBody(Rect rect, SerializedProperty property);
	}
}
