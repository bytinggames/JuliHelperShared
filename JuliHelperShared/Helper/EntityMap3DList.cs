﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuliHelper
{
    public class EntityMap3DList<T> : EntityMap3D<T> where T : IBoundingBox
    {
        public EntityMap3DList(float fieldSize) : base(fieldSize)
        {
        }
        public EntityMap3DList(Vector3 fieldSize) : base(fieldSize)
        {
        }

        public List<T> Entities { get; private set; } = new List<T>();

        public override void Add(T entity)
        {
            Entities.Add(entity);

            base.Add(entity);
        }
        public override bool Remove(T entity)
        {
            bool anyFail = base.Remove(entity);

            Entities.Remove(entity);

            return !anyFail;
        }
    }
}
