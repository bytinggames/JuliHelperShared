﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JuliHelper
{
    public struct Int2 : IComparable<Int2>
    {
        public int X, Y;

        public Int2(int xy)
        {
            this.X = this.Y = xy;
        }
        public Int2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Int2(Vector2 vec)
        {
            this.X = (int)vec.X;
            this.Y = (int)vec.Y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public bool SetInRect(int minX, int minY, int maxX, int maxY)
        {
            int x2 = X;
            int y2 = Y;
            X = Math.Min(maxX, Math.Max(minX, X));
            Y = Math.Min(maxY, Math.Max(minY, Y));

            return x2 != X || y2 != Y;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is Int2)
        //    return x == vint.x && Y == vint.y;
        //}

        public static bool operator ==(Int2 v1, Int2 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }
        public static bool operator !=(Int2 v1, Int2 v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y;
        }

        public static Int2 operator +(Int2 v1, Int2 v2)
        {
            return new Int2(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static Int2 operator -(Int2 v1, Int2 v2)
        {
            return new Int2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static Int2 operator -(Int2 v1)
        {
            return new Int2(-v1.X, -v1.Y);
        }
        public static Int2 operator *(Int2 v1, Int2 v2)
        {
            return new Int2(v1.X * v2.X, v1.Y * v2.Y);
        }
        public static Int2 operator *(Int2 v1, float f2)
        {
            return new Int2((int)(v1.X * f2), (int)(v1.Y * f2));
        }
        public static Int2 operator /(Int2 v1, Int2 v2)
        {
            return new Int2(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static Int2 operator +(Int2 v1, Vector2 v2)
        {
            return new Int2(v1.X + (int)v2.X, v1.Y + (int)v2.Y);
        }

        public static Int2 operator *(Int2 v1, int i2)
        {
            return new Int2(v1.X * i2, v1.Y * i2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return X + " " + Y;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public void SwapXY()
        {
            int save = X;
            X = Y;
            Y = save;
        }

        public int CompareTo([AllowNull] Int2 other)
        {
            int ySign = Math.Sign(Y - other.Y);
            if (ySign == 0)
            {
                return Math.Sign(X - other.X);
            }
            else
                return ySign;
        }
    }
}
