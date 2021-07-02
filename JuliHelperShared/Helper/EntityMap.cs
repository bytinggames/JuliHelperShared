using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuliHelper
{
    public class EntityMap<T> where T : IBoundingRectangle
    {
        public float FieldSize { get; private set; }
        public Dictionary<Int2, List<T>> Grid { get; private set; } = new Dictionary<Int2, List<T>>();
        public List<T> Entities { get; private set; } = new List<T>();

        bool boundsSet = false;
        public int minX, minY, maxX, maxY;

        public EntityMap(float fieldSize)
        {
            FieldSize = fieldSize;
        }

        public void Add(T entity)
        {
            Entities.Add(entity);
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

        public bool Remove(T entity)
        {
            bool anyFail = RemoveFromGrid(entity);
            Entities.Remove(entity);
            return !anyFail;
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

        public IEnumerable<Int2> GetCoords(T entity)
        {
            return GetCoords(entity.GetBoundingRectangle());
        }

        public IEnumerable<T> GetEntities(IEnumerable<Int2> coords)
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

        public IEnumerable<T> GetEntities(T searchingEntity, Vector2 movement)
        {
            return GetEntities(searchingEntity.GetBoundingRectangle().CloneRectangle().Expand(movement));
        }
        public IEnumerable<T> GetEntities(M_Rectangle boundingBox)
        {
            return GetEntities(GetCoords(boundingBox));
        }

        public IEnumerable<Int2> GetCoords(M_Rectangle boundingBox)
        {
            int x1 = (int)Math.Floor(boundingBox.Left / FieldSize);
            int y1 = (int)Math.Floor(boundingBox.Top / FieldSize);
            int x2 = (int)Math.Ceiling(boundingBox.Right / FieldSize);
            int y2 = (int)Math.Ceiling(boundingBox.Bottom / FieldSize);

            Int2 c = new Int2(x1, y1);
            for (; c.Y < y2; c.Y++)
            {
                for (c.X = x1; c.X < x2; c.X++)
                {
                    yield return c;
                }
            }
        }

        private void AddToCoord(Int2 c, T entity)
        {
            if (Grid.TryGetValue(c, out List<T> list))
            {
                list.Add(entity);
            }
            else
            {
                Grid.Add(c, new List<T>() { entity });

                if (!boundsSet)
                {
                    boundsSet = true;
                    minX = maxX = c.X;
                    minY = maxY = c.Y;
                }
                else
                {
                    if (c.X < minX)
                        minX = c.X;
                    else if (c.X > maxX)
                        maxX = c.X;

                    if (c.Y < minY)
                        minY = c.Y;
                    else if (c.Y > maxY)
                        maxY = c.Y;
                }
            }
        }
        private bool RemoveFromCoord(Int2 c, T entity)
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

        public IEnumerable<T> GetEntities(Int2 c)
        {
            if (Grid.TryGetValue(c, out List<T> list))
                return list.AsEnumerable();
            else
                return Enumerable.Empty<T>();
        }

        public IEnumerable<T> GetEntities(int x, int y) => GetEntities(new Int2(x, y));

        public IEnumerable<Int2> GetCoords(Vector2 rayOrigin, Vector2 rayDirection)
        {
            // TODO: get level bounds

            float x = rayOrigin.X / FieldSize;
            float y = rayOrigin.Y / FieldSize;
            float dx = rayDirection.X;
            float dy = rayDirection.Y;
            float dxAbs = Math.Abs(rayDirection.X);
            float dyAbs = Math.Abs(rayDirection.Y);

            int cx = (int)Math.Floor(x);
            int cy = (int)Math.Floor(y);

            while (x >= minX && y >= minY && x < maxX + 1 && y < maxY + 1)
            {
                // parts inside the tile
                float x1 = x - cx;
                float y1 = y - cy;

                yield return new Int2(cx, cy);

                float remainingX = dx >= 0 ? 1f - x1 : x1;
                float remainingY = dy >= 0 ? 1f - y1 : y1;

                float timeToX = remainingX / dxAbs;
                float timeToY = remainingY / dyAbs;

                if (timeToY < timeToX)
                {
                    // move to Y
                    x += timeToY * dx;
                    y += timeToY * dy;
                    y = MathF.Round(y);
                    cy += Math.Sign(dy);
                }
                else
                {
                    // move to X
                    x += timeToX * dx;
                    y += timeToX * dy;
                    x = MathF.Round(x);
                    cx += Math.Sign(dx);
                }

            }
        }
    }
}
