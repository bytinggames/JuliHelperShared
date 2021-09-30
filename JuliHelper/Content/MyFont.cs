using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuliHelper
{
    public class MyFont
    {
        public SpriteFont Font { get; }
        public int DefaultCharacterHeight { get; }
        public int DefaultCharacterYOffset { get; }

        public MyFont(SpriteFont font)
        {
            Font = font;

            var a = font.GetGlyphs()['A'];
            DefaultCharacterHeight = a.BoundsInTexture.Height;
            DefaultCharacterYOffset = a.Cropping.Y;
        }
    }
}
