using UnityEngine;
using Imoet.Unity;

namespace Imoet.Unity {
    [System.Serializable]
    public class Grid
    {
        private Array2D<Cell> m_data;
        private Config m_config;

        public Config config { get { return m_config; } }
        public Cell this[int row, int col] { get { return m_data[row, col]; } }

        //Constructor
        public Grid(Config config)
        {
            _buildGrid(config);
        }

        //Public Function
        public void Rebuild(Config config)
        {
            _buildGrid(config);
        }

        public Cell FindCellByPoint(Vector3 pos, Transform anchor)
        {
            foreach (var data in m_data.get_data)
            {
                if (data.isContainPoint(pos, anchor))
                    return data;
            }
            return Cell.Null;
        }

        //Editor Only
        public void DrawGizmos(bool solid = true, Transform anchor = null)
        {
            if (m_data == null)
                return;

            for (int x = 0; x < m_data.rowCount; x++)
            {
                for (int y = 0; y < m_data.columnCount; y++)
                {
                    var cell = m_data[x, y];
                    var pos = cell.position;
                    if (anchor)
                        pos = anchor.TransformPoint(pos);

                    if (solid)
                        Gizmos.DrawCube(pos, cell.size);
                    else
                        Gizmos.DrawWireCube(pos, cell.size);
                }
            }
        }

        //Private Function
        private void _buildGrid(Config config)
        {
            m_data = new Array2D<Cell>(config.numOfRow, config.numOfColumn);

            var cell = new Cell();
            cell.size = config.cellSize;

            var sumX = cell.size.x * (float)config.numOfColumn;
            var sumZ = cell.size.z * (float)config.numOfRow;

            //MidCenter
            var midPos = new Vector3(sumX, cell.size.y, sumZ) / 2f;
            var startPos = new Vector3(-midPos.x + cell.size.x / 2f, 0, -midPos.z + cell.size.z / 2f);

            for (int x = 0; x < m_data.rowCount; x++)
            {
                for (int y = 0; y < m_data.columnCount; y++)
                {
                    var pos = new Vector3(startPos.x + cell.size.x * y, startPos.y, startPos.z + cell.size.z * x);
                    cell.position = pos;
                    cell.idxRow = x;
                    cell.idxColumn = y;
                    m_data[x, y] = cell;
                }
            }
            m_config = config;
        }

        [System.Serializable]
        public struct Cell
        {
            public int idxRow;
            public int idxColumn;
            public Vector3 position;
            public Vector3 size;

            public bool isContainPoint(Vector3 pos, Transform anchor = null)
            {
                var bound = new Bounds();
                if (anchor)
                    bound.center = anchor.TransformPoint(position);
                else
                    bound.center = position;
                bound.size = size;
                return bound.Contains(pos);
            }

            public static Cell Null
            {
                get { return new Cell() { idxRow = -1, idxColumn = -1 }; }
            }

            public static bool operator ==(Cell a, Cell b)
            {
                return a.idxColumn == b.idxColumn && a.idxRow == b.idxRow && a.position == b.position && a.size == b.size;
            }
            public static bool operator !=(Cell a, Cell b)
            {
                return a.idxColumn != b.idxColumn || a.idxRow != b.idxRow || a.position != b.position || a.size != b.size;
            }
            public override bool Equals(object obj)
            {
                return obj is Cell && (Cell)obj == this;
            }
            public override int GetHashCode()
            {
                return idxRow.GetHashCode() + idxRow.GetHashCode() + position.GetHashCode() + size.GetHashCode();
            }
        }

        public enum Anchor
        {
            //UpperLeft,
            //UpperCente,
            //UpperRight,
            //MiddleLeft,
            MiddleCenter,
            //MiddleRight,
            //LowerLeft,
            //LowerCenter,
            //LowerRight  
        }

        [System.Serializable]
        public struct Config
        {
            public Vector3 cellSize;
            public int numOfColumn;
            public int numOfRow;
            public Anchor anchor;

            public Config(
                int numOfColumn = 1,
                int numOfRow = 1,
                Vector3 cellSize = default,
                Anchor anchor = Anchor.MiddleCenter)
            {
                this.cellSize = cellSize;
                this.numOfColumn = numOfColumn;
                this.numOfRow = numOfRow;
                this.anchor = anchor;
            }

            public static bool operator ==(Config a, Config b)
            {
                return a.cellSize == b.cellSize && a.numOfColumn == b.numOfColumn && a.numOfRow == b.numOfRow && a.anchor == b.anchor;
            }
            public static bool operator !=(Config a, Config b)
            {
                return a.cellSize != b.cellSize || a.numOfColumn != b.numOfColumn || a.numOfRow != b.numOfRow || a.anchor != b.anchor;
            }
            public override bool Equals(object obj)
            {
                return obj is Cell && (Config)obj == this;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}