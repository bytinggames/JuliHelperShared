﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace JuliHelper.ThreeD
{
    public class ModelExtraReader : ContentTypeReader<ModelExtra>
    {
        protected override ModelExtra Read(ContentReader input, ModelExtra existingInstance)
        {
            ModelExtra extra = new ModelExtra();
            extra.Skeleton = input.ReadObject<List<int>>();
            extra.Clips = input.ReadObject<List<AnimationClip>>();
            extra.extension = input.ReadObject<string>();
            return extra;
        }
    }
}
