using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JuliHelper
{
    public struct Int3 : IComparable<Int3>
    {
        private int x, y, z;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        public Int3(int xyz)
        {
            this.x = this.y = this.z = xyz;
        }
        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Int3(Vector3 vec)
        {
            this.x = (int)vec.X;
            this.y = (int)vec.Y;
            this.z = (int)vec.Z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        /// <summary>Returns true, when any value changed.</summary>
        public bool SetInRect(int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
        {
            int x2 = x;
            int y2 = y;
            int z3 = z;
            x = Math.Min(maxX, Math.Max(minX, x));
            y = Math.Min(maxY, Math.Max(minY, y));
            z = Math.Min(maxZ, Math.Max(minZ, z));

            return x2 != x || y2 != y || z3 != z;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is Int3)
        //    return x == vint.x && Y == vint.y;
        //}

        public static bool operator ==(Int3 v1, Int3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }
        public static bool operator !=(Int3 v1, Int3 v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
        }

        public static Int3 operator +(Int3 v1, Int3 v2)
        {
            return new Int3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static Int3 operator -(Int3 v1, Int3 v2)
        {
            return new Int3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Int3 operator -(Int3 v1)
        {
            return new Int3(-v1.X, -v1.Y, -v1.Z);
        }
        public static Int3 operator *(Int3 v1, Int3 v2)
        {
            return new Int3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
        public static Int3 operator *(Int3 v1, float f2)
        {
            return new Int3((int)(v1.X * f2), (int)(v1.Y * f2), (int)(v1.Z * f2));
        }
        public static Int3 operator /(Int3 v1, Int3 v2)
        {
            return new Int3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Int3 operator +(Int3 v1, Vector3 v2)
        {
            return new Int3(v1.X + (int)v2.X, v1.Y + (int)v2.Y, v1.Z + (int)v2.Z);
        }

        public static Int3 operator *(Int3 v1, int i2)
        {
            return new Int3(v1.x * i2, v1.y * i2, v1.z * i2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return x + " " + y + " " + z;
        }

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public void SwapXY()
        {
            int save = x;
            x = y;
            y = save;
        }

        public void SwapXZ()
        {
            int save = x;
            x = z;
            z = save;
        }

        public void SwapYZ()
        {
            int save = y;
            y = z;
            z = save;
        }

        public int CompareTo([AllowNull] Int3 other)
        {
            int zSign = Math.Sign(z - other.z);
            if (zSign == 0)
            {
                int ySign = Math.Sign(y - other.y);
                if (ySign == 0)
                {
                    return Math.Sign(x - other.x);
                }
                else
                    return ySign;
            }
            else
                return zSign;
        }
    }
}
