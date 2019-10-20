using System;
namespace Imoet.Unity {
    [Serializable]
    public class Array2D<T>
    {
        private int m_dataLength;

        public Array2D(int rowCount, int columnCount) {
            Resize(rowCount, columnCount);
        }

        //Property
        public int rowCount { get; private set; }
        public int columnCount { get; private set; }
        public T this[int row, int coloum] {
            get
            {
                if (row > rowCount - 1 || coloum > columnCount - 1)
                    throw new IndexOutOfRangeException("row or coloum value is out of range");
                return get_data[coloum + row * columnCount];
            }
            set
            {
                if (row > rowCount - 1 || coloum > columnCount - 1)
                    throw new IndexOutOfRangeException("row or coloum value is out of range");
                get_data[coloum + row * columnCount] = value;
            }
        }

        internal T[] get_data { get; private set; }
        internal void __internal_setData(T[] data) {
            get_data = data;
            m_dataLength = data.Length;
        }
        public void Resize(int newRowCount, int newColumnCount) {
            rowCount = newRowCount;
            columnCount = newColumnCount;
            m_dataLength = rowCount * columnCount;
            get_data = new T[m_dataLength];
        }
    }
}