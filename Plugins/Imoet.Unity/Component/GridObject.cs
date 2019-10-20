using UnityEngine;
namespace Imoet.Unity {
    public class GridObject : MonoBehaviour
    {
        [SerializeField]
        private Grid.Config m_config;
        private Grid m_grid;

        public Grid.Cell GetLocalCell(Vector3 pos) {
            return m_grid.FindCellByPoint(pos, transform);
        }
        public Grid.Cell GetWorldCell(Vector3 pos) {
            return m_grid.FindCellByPoint(pos);
        }

        public Vector3 CellToWorld(Grid.Cell cell) {
            var c = m_grid[cell.idxRow, cell.idxColumn];
            if (c == cell)
                return transform.TransformPoint(cell.position);
            return cell.position;
        }
        public Vector3 CellToLocal(Grid.Cell cell) {
            var c = m_grid[cell.idxRow, cell.idxColumn];
            if (c == cell)
                return transform.InverseTransformPoint(cell.position);
            return cell.position;
        }

        private void OnDrawGizmos()
        {
            if (m_grid == null || m_grid.config != m_config) {
                m_grid = new Grid(m_config);
            }
            m_grid.DrawGizmos(false, transform);
        }
    }
}

