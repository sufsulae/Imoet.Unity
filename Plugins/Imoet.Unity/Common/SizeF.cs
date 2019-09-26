using UnityEngine;

namespace Imoet.Unity
{
    public struct SizeF
    {
        public int width;
        public int height;

        public SizeF(int width, int height) {
            this.width = width;
            this.height = height;
        }

        public SizeF(int value) {
            this.width = this.height = value;
        }

        #region Property
        public float ratio {
            get { return (float)width / (float)height; }
        }
        #endregion

        #region Basic Operator
        public static SizeF operator +(SizeF a, SizeF b)
        {
            return new SizeF(a.width + b.width, a.height + b.height);
        }
        public static SizeF operator -(SizeF a, SizeF b)
        {
            return new SizeF(a.width - b.width, a.height - b.height);
        }
        public static SizeF operator *(SizeF a, int b)
        {
            return new SizeF(a.width * b, a.height * b);
        }
        public static SizeF operator *(int a, SizeF b)
        {
            return new SizeF(a * b.width, a * b.height);
        }
        public static SizeF operator *(SizeF a, float b)
        {
            return a * (int)b;
        }
        public static SizeF operator *(float a, SizeF b)
        {
            return b * (int)a;
        }
        public static SizeF operator /(SizeF a, int b)
        {
            return new SizeF(a.width / b, a.height / b);
        }
        public static SizeF operator /(SizeF a, float b)
        {
            return a / (int)b;
        }
        public static SizeF operator -(SizeF a)
        {
            return a * -1;
        }
        #endregion

        #region Implicit Operator
        public static implicit operator Vector2(SizeF p)
        {
            return new Vector2(p.width, p.height);
        }
        public static implicit operator SizeF(Vector2 p)
        {
            return new SizeF((int)p.x, (int)p.y);
        }
#if UNITY_2017_1_OR_NEWER
        public static implicit operator Vector2Int(SizeF p)
        {
            return new Vector2Int(p.width, p.height);
        }
        public static implicit operator SizeF(Vector2Int p)
        {
            return new SizeF(p.x, p.y);
        }
#endif
        #endregion

#region Equal Operator
        public static bool operator ==(SizeF left, SizeF right)
        {
            return left.width == right.width && left.height == right.height;
        }
        public static bool operator !=(SizeF left, SizeF right)
        {
            return left.width != right.width || left.height == right.height;
        }
#endregion

        public override bool Equals(object obj)
        {
            if (obj is SizeF)
                return this == (SizeF)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return this.width.GetHashCode() + this.height.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("x:{0} y:{1}", this.width, this.height);
        }
    }
}

