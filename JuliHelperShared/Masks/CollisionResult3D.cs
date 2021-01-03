using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace JuliHelper
{
    public struct CollisionResult3D
    {
        public bool collision;
        public float? distance;
        public float? distanceReversed;
        public Vector3 axisCol, axisColReversed, colPoint, colPointReversed;

        public void AxisInvert()
        {
            axisCol = -axisCol;
            axisColReversed = -axisColReversed;
        }

        public CollisionResult3D GetAxisInvert()
        {
            AxisInvert();
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

            if (!distanceReversed.HasValue || (cr.distanceReversed.HasValue && cr.distanceReversed < distanceReversed))
            {
                distanceReversed = cr.distanceReversed;
                axisColReversed = cr.axisColReversed;
                colPointReversed = cr.colPointReversed;
            }

            if (!distance.HasValue || (cr.distance.HasValue && cr.distance < distance))
            {
                distance = cr.distance;
                axisCol = cr.axisCol;
                colPoint = cr.colPoint;

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
                distance = cr.distance;
                axisCol = cr.axisCol;
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
                distance = cr.distance;
                axisCol = cr.axisCol;
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
                    distance = cr.distance;
                    axisCol = cr.axisCol;
                    colPoint = cr.colPoint;
                }

                if (!distanceReversed.HasValue || cr.distance.Value > distanceReversed.Value)
                {
                    distanceReversed = cr.distance;
                    axisColReversed = cr.axisCol;
                    colPointReversed = cr.colPoint;
                }
            }
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
