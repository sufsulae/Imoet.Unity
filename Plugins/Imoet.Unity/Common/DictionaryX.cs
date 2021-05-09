using System.Collections;
using System.Collections.Generic;

namespace Imoet.Unity {
	[System.Serializable]
	public class DictionaryX<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> {
		private List<KeyValuePair<TKey, TValue>> m_list;

		public int Count { get { return m_list.Count; } }
		public TValue this[TKey key] { get { return _getByKey(key).Value; } }
		public TKey[] Keys {
			get {
				var value = new TKey[m_list.Count];
				for (int i = 0; i < m_list.Count; i++)
					value[i] = m_list[i].Key;
				return value;
			}
		}
		public TValue[] Values {
			get {
				var value = new TValue[m_list.Count];
				for (int i = 0; i < m_list.Count; i++)
					value[i] = m_list[i].Value;
				return value;
			}
		}

		public DictionaryX() {
			m_list = new List<KeyValuePair<TKey, TValue>>();
		}

		public void Add(TKey key, TValue value) {
			_checkKey(key);
			m_list.Add(new KeyValuePair<TKey, TValue>(key, value));
		}
		public bool Remove(TKey key) {
			for (int i = 0; i < m_list.Count; i++) {
				if (m_list[i].Key.Equals(key)) {
					m_list.RemoveAt(i);
					return true;
				}
			}
			return false;
		}
		public void RemoveAt(int idx) {
			m_list.RemoveAt(idx);
		}
		public void Insert(int idx, TKey key, TValue value) {
			_checkKey(key);
			m_list.Insert(idx, new KeyValuePair<TKey, TValue>(key, value));
		}
		public KeyValuePair<TKey, TValue> Get(int idx) {
			return m_list[idx];
		}
		public void Set(int idx, KeyValuePair<TKey, TValue> value) {
			m_list[idx] = value;
		}
		public void Set(int idx, TKey key, TValue value) {
			m_list[idx] = new KeyValuePair<TKey, TValue>(key, value);
		}
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return m_list.GetEnumerator();
		}

		private void _checkKey(TKey key) {
			foreach (var i in m_list) {
				if (i.Key.Equals(key))
					throw new System.InvalidOperationException("Key Already Exist");
			}
		}
		private KeyValuePair<TKey, TValue> _getByKey(TKey key) {
			foreach (var i in m_list) {
				if (i.Key.Equals(key))
					return i;
			}
			return default(KeyValuePair<TKey,TValue>);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_list.GetEnumerator();
		}
	}
}

