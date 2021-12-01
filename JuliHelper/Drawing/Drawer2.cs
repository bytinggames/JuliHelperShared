using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JuliHelper
{
    public class PrimitiveLineRing
    {
        public Vector2[] Vertices { get; set; }

        public PrimitiveLineRing(Vector2[] vertices)
        {
            this.Vertices = vertices;
        }

        public PrimitiveLineRing(PrimitiveAreaStrip area)
        {
            if (area.Vertices.Length < 3)
                throw new Exception("TriangleStrip with less than 3 vertices is not allowed");

            Vertices = new Vector2[area.Vertices.Length];
            // start edge
            Vertices[0] = area.Vertices[0];
            Vertices[1] = area.Vertices[1];
            // bottom line
            int i;
            int j;
            for (i = 2, j = 3; j < area.Vertices.Length; i++, j += 2)
                Vertices[i] = area.Vertices[j];
            // top line
            for (j = area.Vertices.Length - 2; j > 0; i++, j -= 2)
                Vertices[i] = area.Vertices[j];
        }

        public PrimitiveLineRing(M_Circle circle, int vertexCount)
        {
            Vertices = new Vector2[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                float angle = MathHelper.TwoPi * i / vertexCount;
                Vertices[i] = circle.pos + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * circle.radius;
            }
        }

        public PrimitiveLineRing(M_Rectangle rect)
        {
            Vertices = new Vector2[]
            {
                rect.TopRight,
                rect.TopLeft,
                rect.BottomLeft,
                rect.BottomRight
            };
        }

        public PrimitiveAreaRing Thicken(float thickness)
        {
            return new PrimitiveAreaRing(this, thickness);
        }
        public PrimitiveAreaRing ThickenOutside(float thickness)
        {
            return new PrimitiveAreaRing(this, thickness, 1f);
        }
        public PrimitiveAreaRing ThickenInside(float thickness)
        {
            return new PrimitiveAreaRing(this, thickness, -1f);
        }
        public PrimitiveAreaRing Thicken(float thickness, float anchorInner)
        {
            return new PrimitiveAreaRing(this, thickness, anchorInner);
        }
    }

    public class PrimitiveAreaRing
    {
        public Vector2[] Vertices { get; set; }

        public PrimitiveAreaRing(PrimitiveLineRing ring, float thickness, float anchor = 0f)
        {
            Vertices = new Vector2[ring.Vertices.Length * 2];

            float t = thickness / 2f;

            for (int i = 0; i < ring.Vertices.Length; i++)
            {
                Vector2 a = ring.Vertices[i] - ring.Vertices[(i + ring.Vertices.Length - 1) % ring.Vertices.Length];
                Vector2 b = ring.Vertices[i] - ring.Vertices[(i + 1) % ring.Vertices.Length];
                float angleA = MathF.Atan2(a.Y, a.X);
                float angleB = MathF.Atan2(b.Y, b.X);
                float angleDistHalved = Calculate.AngleDistance(angleA, angleB) / 2f;
                float sin = MathF.Sin(angleDistHalved);
                float x = t / sin;
                float angleToCorner = angleA + angleDistHalved;
                Vector2 dirToCorner = new Vector2(MathF.Cos(angleToCorner), MathF.Sin(angleToCorner)) * x;

                Vertices[i * 2] = ring.Vertices[i] + dirToCorner * (1f - anchor);
                Vertices[i * 2 + 1] = ring.Vertices[i] + dirToCorner * (-1f - anchor);
            }
        }
        public void Draw(GraphicsDevice gDevice)
        {
            var arr = GetVertexPositions();
            gDevice.DrawUserPrimitives<VertexPosition>(PrimitiveType.TriangleStrip, arr, 0, arr.Length - 2);
        }

        public VertexPosition[] GetVertexPositions()
        {
            return Vertices.Select(f => new VertexPosition(new Vector3(f, 0f)))
                .Concat(new List<VertexPosition>()
                {
                    new VertexPosition(new Vector3(Vertices[0], 0f)),
                    new VertexPosition(new Vector3(Vertices[1], 0f))
                })
                .ToArray();
        }
    }

    public class PrimitiveAreaStrip : PrimitiveArea
    {
        public Vector2[] Vertices { get; set; }

        public PrimitiveAreaStrip(Vector2[] vertices)
        {
            this.Vertices = vertices;
        }

        public PrimitiveAreaStrip(M_Rectangle rect)
        {
            Vector2 p1 = rect.pos;
            Vector2 p2 = rect.pos + rect.size;
            Vertices = new Vector2[4];
            Vertices[0] = new Vector2(p1.X, p2.Y);
            Vertices[1] = p1;
            Vertices[2] = p2;
            Vertices[3] = new Vector2(p2.X, p1.Y);
        }

        //public PrimitiveArea Outline(float thickness)
        //{

        //}

        public override PrimitiveLineRing Outline()
        {
            return new PrimitiveLineRing(this);
        }

        public override void Draw(GraphicsDevice gDevice)
        {
            //var v = new VertexBuffer(gDevice, new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0)), Vertices.Length, BufferUsage.WriteOnly);

            //v.SetData(Vertices);

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i = 0; i < 1000; i++)
            //{
            //    gDevice.SetVertexBuffer(v);
            //    gDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, Vertices.Length - 2);

            //}
            //sw.Stop();

            var arr = Vertices.Select(f => new VertexPosition(new Vector3(f, 0f))).ToArray();
            //sw.Restart();
            //for (int i = 0; i < 1000; i++)
            //{
                gDevice.DrawUserPrimitives<VertexPosition>(PrimitiveType.TriangleStrip, arr, 0, Vertices.Length - 2);
            //}
            //sw.Stop();
        }
    }

    public abstract class PrimitiveArea
    {
        public abstract void Draw(GraphicsDevice gDevice);

        public abstract PrimitiveLineRing Outline();

        public PrimitiveAreaRing Outline(float thickness)
        {
            return Outline().Thicken(thickness);
        }
        public PrimitiveAreaRing Outline(float thickness, float anchor = 0f)
        {
            return Outline().Thicken(thickness, anchor);
        }
        public PrimitiveAreaRing OutlineInside(float thickness)
        {
            return Outline().ThickenInside(thickness);
        }
        public PrimitiveAreaRing OutlineOutside(float thickness)
        {
            return Outline().ThickenOutside(thickness);
        }
    }

    public class PrimitiveAreaFan : PrimitiveArea
    {
        public Vector2[] Vertices { get; set; }

        public PrimitiveAreaFan(Vector2[] vertices)
        {
            this.Vertices = vertices;
        }

        public PrimitiveAreaFan(M_Circle circle, int outerVertexCount)
        {
            Vertices = new Vector2[outerVertexCount + 1];
            Vertices[0] = circle.pos;

            for (int i = 0; i < outerVertexCount; i++)
            {
                float angle = MathHelper.TwoPi * i / outerVertexCount;
                Vertices[i + 1] = circle.pos + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * circle.radius;
            }
        }

        public override PrimitiveLineRing Outline()
        {
            return new PrimitiveLineRing(Vertices.Skip(1).ToArray());
        }

        public override void Draw(GraphicsDevice gDevice)
        {
            var v = Vertices.Select(f => new VertexPosition(new Vector3(f, 0f))).ToArray();
            short[] indices = new short[(Vertices.Length - 1) * 3];
            short triCount = (short)(indices.Length / 3);
            short i;
            for (i = 0; i < triCount - 1; i++)
            {
                indices[i * 3 + 1] = (short)(i + 1);
                indices[i * 3 + 2] = (short)(i + 2);
            }
            indices[i * 3 + 1] = (short)(i + 1);
            indices[i * 3 + 2] = 1;

            gDevice.DrawUserIndexedPrimitives<VertexPosition>(PrimitiveType.TriangleList, v, 0, v.Length, indices, 0, triCount);
        }
    }

    public static class Drawer2
    {
        // getting shapes
        // GetRectangle()
        // GetCircle()
        // GetPolygon()
        // GetLine()



        // filling shapes
        // extruding shapes (rounded corners possible)
        // scaling shapes (for making outlines outside or inside)
        // outlineOutside, outlineInside, outline

        // example
        //public void Test()
        //{
        //    new Primitive(new M_Rectangle(0,0,100,100)).Outline(2f).Draw();
        //}

        public static PrimitiveAreaStrip ToPrimitiveArea(this M_Rectangle rect) => new PrimitiveAreaStrip(rect);
        public static PrimitiveLineRing ToPrimitiveLine(this M_Rectangle rect) => new PrimitiveLineRing(rect);
        public static PrimitiveAreaFan ToPrimitiveArea(this M_Circle circle, int vertexCount) => new PrimitiveAreaFan(circle, vertexCount);
        public static PrimitiveLineRing ToPrimitiveLine(this M_Circle circle, int vertexCount) => new PrimitiveLineRing(circle, vertexCount);
    }
}
