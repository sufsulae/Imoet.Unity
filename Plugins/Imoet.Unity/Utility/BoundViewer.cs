using UnityEngine;

namespace Imoet.Unity.Utility {
	public class BoundViewer : MonoBehaviour
	{
		[SerializeField]
		private bool m_viewBound = true;
		[SerializeField]
		private bool m_filled = false;
		[SerializeField]
		private Color m_color = Color.white;

		private Collider m_collider;
		private Collider2D m_collider2D;
		private Renderer m_renderer;

		private void OnDrawGizmos()
		{
			if (m_viewBound)
			{
				if (m_collider == null)
					m_collider = GetComponent<Collider>();
				else
				{
					Gizmos.color = m_color;
					var bounds = m_collider.bounds;
					if (m_filled)
						Gizmos.DrawCube(bounds.center, bounds.size);
					else
						Gizmos.DrawWireCube(bounds.center, bounds.size);
					return;
				}

				if (m_collider2D == null)
					m_collider2D = GetComponent<Collider2D>();
				else
				{
					Gizmos.color = m_color;
					var bounds = m_collider2D.bounds;
					if (m_filled)
						Gizmos.DrawCube(bounds.center, bounds.size);
					else
						Gizmos.DrawWireCube(bounds.center, bounds.size);
					return;
				}

				if (m_renderer == null)
					m_renderer = GetComponent<Renderer>();
				else
				{
					Gizmos.color = m_color;
					var bounds = m_renderer.bounds;
					if (m_filled)
						Gizmos.DrawCube(bounds.center, bounds.size);
					else
						Gizmos.DrawWireCube(bounds.center, bounds.size);
					return;
				}
			}
		}
	}
}
