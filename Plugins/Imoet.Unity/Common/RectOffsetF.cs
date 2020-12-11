namespace Imoet.Unity
{
    [System.Serializable]
    public sealed class RectOffsetF
    {
        public float left, right, top, bottom;
        public RectOffsetF(float top, float right, float bottom, float left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }

        public float this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0: return top;
                    case 1: return right;
                    case 2: return bottom;
                    case 3: return left;
                }
                throw new System.IndexOutOfRangeException();
            }
        }

        public static bool operator ==(RectOffsetF l, RectOffsetF r)
        {
            for (int i = 0; i < 4; i++)
                if (l[i] != r[i])
                    return false;
            return true;
        }
        public static bool operator !=(RectOffsetF l, RectOffsetF r)
        {
            for (int i = 0; i < 4; i++)
                if (l[i] == r[i])
                    return false;
            return true;
        }
        public override int GetHashCode()
        {
            return left.GetHashCode() + right.GetHashCode() + top.GetHashCode() + bottom.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is RectOffsetF)
                return (RectOffsetF)obj == this;
            return false;
        }
        public override string ToString()
        {
            return string.Format("top:{0}, right:{1}, bottom:{2}, left:{3}", top, right, bottom, left);
        }
    }
}
