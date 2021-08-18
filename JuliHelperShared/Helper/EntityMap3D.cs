﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuliHelper
{
    public class EntityMap3D<T> where T : IBoundingBox
    {
        public Vector3 FieldSize { get; private set; }
        public Dictionary<Int3, List<T>> Grid { get; private set; } = new Dictionary<Int3, List<T>>();

        public bool BoundsSet { get; private set; } = false;
        private Int3 min;
        private Int3 max;

        public Int3 Min => min;
        public Int3 Max => max;

        public EntityMap3D(float fieldSize)
        {
            FieldSize = new Vector3(fieldSize);
        }

        public EntityMap3D(Vector3 fieldSize)
        {
            FieldSize = fieldSize;
        }

        public virtual void Add(T entity)
        {
            AddToGrid(entity);
        }

        /// <summary>Does not add to Entities list.</summary>
        private void AddToGrid(T entity)
        {
            foreach (var c in GetCoords(entity))
                AddToCoord(c, entity);
        }

        public void ChangePos(T entity, Action<T> changePosAction)
        {
            if (!RemoveFromGrid(entity))
                throw new Exception("Error in map storage: Couldn't find entity in some coordinations where it should have been.");

            changePosAction(entity);

            AddToGrid(entity);
        }

        public virtual bool Remove(T entity)
        {
            return !RemoveFromGrid(entity);
        }

        /// <summary>Does not remove from Entities list.</summary>
        private bool RemoveFromGrid(T entity)
        {
            bool anyFail = false;
            foreach (var c in GetCoords(entity))
            {
                if (!RemoveFromCoord(c, entity))
                    anyFail = true;
            }
            return !anyFail;
        }

        public IEnumerable<Int3> GetCoords(T entity)
        {
            return GetCoords(entity.GetBoundingBox());
        }

        public IEnumerable<T> GetEntities(IEnumerable<Int3> coords)
        {
            HashSet<T> done = new HashSet<T>();
            foreach (var c in coords)
            {
                foreach (var e in GetEntities(c))
                {
                    if (!done.Contains(e))
                    {
                        done.Add(e);
                        yield return e;
                    }
                }
            }
        }

        public IEnumerable<T> GetEntities(T searchingEntity, Vector3 movement)
        {
            return GetEntities(searchingEntity.GetBoundingBox().Expand(movement));
        }
        public IEnumerable<T> GetEntities(BoundingBox boundingBox)
        {
            return GetEntities(GetCoords(boundingBox));
        }

        public IEnumerable<Int3> GetCoords(BoundingBox boundingBox)
        {
            int x1 = (int)Math.Floor(boundingBox.Min.X / FieldSize.X);
            int y1 = (int)Math.Floor(boundingBox.Min.Y / FieldSize.Y);
            int z1 = (int)Math.Floor(boundingBox.Min.Z / FieldSize.Z);
            int x2 = (int)Math.Ceiling(boundingBox.Max.X / FieldSize.X);
            int y2 = (int)Math.Ceiling(boundingBox.Max.Y / FieldSize.Y);
            int z2 = (int)Math.Ceiling(boundingBox.Max.Z / FieldSize.Z);

            Int3 c = new Int3(x1, y1, z1);
            for (; c.Z < z2; c.Z++)
            {
                for (; c.Y < y2; c.Y++)
                {
                    for (c.X = x1; c.X < x2; c.X++)
                    {
                        yield return c;
                    }
                }
            }
        }

        private void AddToCoord(Int3 c, T entity)
        {
            if (Grid.TryGetValue(c, out List<T> list))
            {
                list.Add(entity);
            }
            else
            {
                Grid.Add(c, new List<T>() { entity });

                if (!BoundsSet)
                {
                    min = max = c;
                    BoundsSet = true;
                }
                else
                {
                    if (c.X < min.X)
                        min.X = c.X;
                    else if (c.X > max.X)
                        max.X = c.X;

                    if (c.Y < min.Y)
                        min.Y = c.Y;
                    else if (c.Y > max.Y)
                        max.Y = c.Y;

                    if (c.Z < min.Z)
                        min.Z = c.Z;
                    else if (c.Z > max.Z)
                        max.Z = c.Z;
                }
            }
        }
        private bool RemoveFromCoord(Int3 c, T entity)
        {
            if (Grid.TryGetValue(c, out List<T> list))
            {
                list.Remove(entity);

                // if list is empty, remove entry from dictionary
                if (list.Count == 0)
                    Grid.Remove(c);

                return true;
            }
            return false;
        }

        public IEnumerable<T> GetEntities(Int3 c)
        {
            if (Grid.TryGetValue(c, out List<T> list))
                return list.AsEnumerable();
            else
                return Enumerable.Empty<T>();
        }

        public IEnumerable<T> GetEntities(int x, int y, int z) => GetEntities(new Int3(x, y, z));

        public IEnumerable<Int3> GetCoords(Vector3 rayOrigin, Vector3 rayDirection)
        {
            // TODO: get level bounds

            float x = rayOrigin.X / FieldSize.X;
            float y = rayOrigin.Y / FieldSize.Y;
            float z = rayOrigin.Z / FieldSize.Z;
            float dx = rayDirection.X;
            float dy = rayDirection.Y;
            float dz = rayDirection.Z;
            float dxAbs = Math.Abs(rayDirection.X);
            float dyAbs = Math.Abs(rayDirection.Y);
            float dzAbs = Math.Abs(rayDirection.Z);

            int cx = (int)Math.Floor(x);
            int cy = (int)Math.Floor(y);
            int cz = (int)Math.Floor(z);

            while (x >= min.X && y >= min.Y  && z >= min.Z
                && x < max.X + 1 && y < max.Y + 1 && z < max.Z + 1)
            {
                // parts inside the tile
                float x1 = x - cx;
                float y1 = y - cy;
                float z1 = z - cz;

                yield return new Int3(cx, cy, cz);

                float remainingX = dx >= 0 ? 1f - x1 : x1;
                float remainingY = dy >= 0 ? 1f - y1 : y1;
                float remainingZ = dz >= 0 ? 1f - z1 : z1;

                float timeToX = remainingX / dxAbs;
                float timeToY = remainingY / dyAbs;
                float timeToZ = remainingZ / dzAbs;

                if (timeToY < timeToX)
                {
                    if (timeToY < timeToZ)
                        MoveToY();
                    else
                        MoveToZ();
                }
                else
                {
                    if (timeToX < timeToZ)
                        MoveToX();
                    else
                        MoveToZ();
                }

                void MoveToX()
                {
                    x += timeToX * dx;
                    y += timeToX * dy;
                    z += timeToZ * dz;
                    x = MathF.Round(x);
                    cx += Math.Sign(dx);
                }

                void MoveToY()
                {
                    x += timeToY * dx;
                    y += timeToY * dy;
                    z += timeToY * dz;
                    y = MathF.Round(y);
                    cy += Math.Sign(dy);
                }

                void MoveToZ()
                {
                    x += timeToX * dx;
                    y += timeToX * dy;
                    z += timeToZ * dz;
                    z = MathF.Round(z);
                    cz += Math.Sign(dz);
                }
            }
        }
    }
}