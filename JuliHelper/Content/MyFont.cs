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
        public SpriteFont Font { get; private set; }
        public int DefaultCharacterHeight { get; private set; }
        public int DefaultCharacterYOffset { get; private set; }

        public event Action OnReload;

        public MyFont(SpriteFont font)
        {
            ReloadFontInternal(font, true);
        }

        public void ReloadFont(SpriteFont font) => ReloadFontInternal(font, false);

        private void ReloadFontInternal(SpriteFont font, bool constructor)
        {
            Font = font;

            var a = font.GetGlyphs()['A'];
            DefaultCharacterHeight = a.BoundsInTexture.Height;
            DefaultCharacterYOffset = a.Cropping.Y;

            if (!constructor)
                OnReload?.Invoke();
        }
    }
}
