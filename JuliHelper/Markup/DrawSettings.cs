using JuliHelper.Markup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace JuliHelper.Markup
{
    public class DrawSettings : ICloneable
    {
        public MyFont MyFont { get; set; }
        public Anchor Anchor { get; set; }
        public Color TextColor { get; set; }
        public float HorizontalAlignInLine { get; set; }
        public float VerticalAlignInLine { get; set; } = 0.5f;
        public Vector2 Scale { get; set; }
        public float Rotation { get; }
        public SpriteEffects Effects { get; }
        public long Time { get; set; }

        public DrawSettings(MyFont myFont, Anchor anchor, Color? textColor = null, float align = 0.5f, Vector2? scale = null, float rotation = 0f, SpriteEffects effects = SpriteEffects.None)
        {
            MyFont = myFont;
            Anchor = anchor;
            TextColor = textColor ?? Color.Black;
            HorizontalAlignInLine = align;
            Scale = scale ?? Vector2.One;
            Rotation = rotation;
            Effects = effects;
        }

        public DrawSettings CloneDrawSettings()
        {
            DrawSettings clone = (DrawSettings)this.MemberwiseClone();
            clone.Anchor = Anchor.Clone();
            return clone;
        }
        public object Clone() => CloneDrawSettings();
    }
}
