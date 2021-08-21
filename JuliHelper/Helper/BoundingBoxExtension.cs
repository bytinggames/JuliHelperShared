using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelper
{
    static class BoundingBoxExtension
    {
        public static BoundingBox Expand(this BoundingBox box, Vector3 expand)
        {
            if (expand.X > 0)
                box.Max.X += expand.X;
            else
                box.Min.X += expand.X;

            if (expand.Y > 0)
                box.Max.Y += expand.Y;
            else
                box.Min.Y += expand.Y;

            if (expand.Z > 0)
                box.Max.Z += expand.Z;
            else
                box.Min.Z += expand.Z;

            return box;
        }
    }
}
