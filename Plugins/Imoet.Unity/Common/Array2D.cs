using System;
namespace Imoet.Unity {
    [Serializable]
    public class Array2D<T>
    {
        private int m_dataLength;

        public Array2D(int rowCount, int coloumCount)
        {
            Resize(rowCount, coloumCount);
        }
        //Property
        public int rowCount { get; private set; }

        public int coloumCount { get; private set; }

        public T this[int row, int coloum]
        {
            get
            {
                if (row > rowCount - 1 || coloum > coloumCount - 1)
                    throw new IndexOutOfRangeException("row or coloum value is out of range");
                return get_data[coloum + row * rowCount];
            }
            set
            {
                if (row > rowCount - 1 || coloum > coloumCount - 1)
                    throw new IndexOutOfRangeException("row or coloum value is out of range");
                get_data[coloum + row * rowCount] = value;
            }
        }

        internal T[] get_data { get; private set; }

        public void Resize(int newRowCount, int newColoumCount)
        {
            rowCount = newRowCount;
            coloumCount = newColoumCount;
            m_dataLength = rowCount * coloumCount;
            get_data = new T[m_dataLength];
        }
    }
}