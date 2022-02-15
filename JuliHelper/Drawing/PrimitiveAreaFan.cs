﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace JuliHelper
{
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
}
