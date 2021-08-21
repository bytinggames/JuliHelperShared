using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuliHelper
{
    public class EntityMapList<T> : EntityMap<T> where T : IBoundingRectangle
    {
        public EntityMapList(float fieldSize)
            :base(fieldSize)
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
