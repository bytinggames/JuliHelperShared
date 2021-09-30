using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JuliHelper
{
    public class CollisionResult3D
    {
        public bool collision;
        public float? distance;
        public float? distanceReversed;
        public Vector3 axisCol, axisColReversed;
        /// <summary>
        /// is calculated by letting one shape move towards another shape, that does not move.
        /// </summary>
        public Vector3? colPoint;

        /// <summary>
        /// -1: none
        /// 0,1,2: vertex.
        /// 3,4,5: edge.
        /// 6: face</summary>
        public int colTriangleIndex = -1;

        public void AxisInvert()
        {
            axisCol = -axisCol;
            axisColReversed = -axisColReversed;
            colPoint = null; // use AxisInvert(Vector3) for keeping colPoint
        }

        public void AxisInvert(Vector3 newDir)
        {
            // set colPoint to the collision of the other shape
            if (distance.HasValue)
            {
                if (colPoint.HasValue)
                {
                    colPoint += newDir * distance.Value;
                }
            }
            else
                colPoint = null;

            axisCol = -axisCol;
            axisColReversed = -axisColReversed;
        }

        public CollisionResult3D GetAxisInvert()
        {
            AxisInvert();
            return this;
        }

        public CollisionResult3D GetAxisInvert(Vector3 newDir)
        {
            AxisInvert(newDir);
            return this;
        }

        public override string ToString()
        {
            return "collision: " + collision
                 + "\ndistance: " + distance
                 + "\ndistanceReversed: " + distanceReversed
                 + "\naxisCol: " + axisCol
                 + "\naxisColReversed: " + axisColReversed;
        }

        public void SetCollisionFromDist()
        {
            collision = Math.Sign(distance.Value * distanceReversed.Value) == -1;
        }

        public bool MinResult(CollisionResult3D cr)
        {
            if (cr.collision)
                collision = true;

            if (!distanceReversed.HasValue || (cr.distanceReversed.HasValue && cr.distanceReversed > distanceReversed))
            {
                distanceReversed = cr.distanceReversed;
                axisColReversed = cr.axisColReversed;
            }

            if ((!distance.HasValue && cr.distance.HasValue) || (cr.distance.HasValue && cr.distance < distance))
            {
                CopyForwardValues(cr);

                return true;
            }
            return false;
        }

        public void MaxResult(CollisionResult3D cr)
        {
            if (cr.collision)
                collision = true;

            if (!distance.HasValue || (cr.distance.HasValue && cr.distance > distance))
            {
                CopyForwardValues(cr);
            }

            if (!distanceReversed.HasValue || (cr.distanceReversed.HasValue && cr.distanceReversed > distanceReversed))
            {
                distanceReversed = cr.distanceReversed;
                axisColReversed = cr.axisColReversed;
            }
        }

        /// <summary>
        /// just for testing first (ray test)
        /// </summary>
        /// <param name="cr"></param>
        public void MaxMinResult(CollisionResult3D cr)
        {
            if (!cr.collision)
                collision = false;

            if (!cr.distance.HasValue)
            {
                distance = null;
                axisCol = Vector3.Zero;
            }
            else if (!distance.HasValue || (cr.distance.HasValue && cr.distance > distance))
            {
                CopyForwardValues(cr);
            }

            if (!cr.distanceReversed.HasValue)
            {
                distanceReversed = null;
                axisColReversed = Vector3.Zero;
            }
            else if (!distanceReversed.HasValue || (cr.distanceReversed.HasValue && cr.distanceReversed < distanceReversed))
            {
                distanceReversed = cr.distanceReversed;
                axisColReversed = cr.axisColReversed;
            }
        }


        public void MinResultTakeNormalAsReversed(CollisionResult3D cr)
        {
            if (cr.collision)
                collision = true;

            if (cr.distance.HasValue)
            {
                if (!distance.HasValue || cr.distance.Value < distance.Value)
                {
                    CopyForwardValues(cr);
                }

                if (!distanceReversed.HasValue || cr.distance.Value > distanceReversed.Value)
                {
                    distanceReversed = cr.distance;
                    axisColReversed = cr.axisCol;
                }
            }
        }

        private void CopyForwardValues(CollisionResult3D cr)
        {
            distance = cr.distance;
            axisCol = cr.axisCol;
            colPoint = cr.colPoint;
            colTriangleIndex = cr.colTriangleIndex;
        }

        public bool MinResultIfCollisionInPresentOrFuture(CollisionResult3D cr)
        {
            if (cr.distanceReversed.HasValue)
            {
                // is collision in positive distance?
                // or is a collision happening right now?
                if (cr.distanceReversed >= 0)
                {
                    return MinResult(cr);
                }
            }
            return false;
        }
    }
}
