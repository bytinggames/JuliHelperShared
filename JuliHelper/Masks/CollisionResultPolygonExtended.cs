using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JuliHelper
{
    public class CollisionResultPolygonExtended : CollisionResult
    {
        public float colVertexIndex = -1f;
        ///// <summary>
        ///// is calculated by letting one shape move towards another shape, that does not move.
        ///// </summary>
        //public Vector2? colPoint;
    }
}
