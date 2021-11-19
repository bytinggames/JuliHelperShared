using JuliHelper.Markup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace JuliHelper.Markup
{
    public class MarkupSettings : ICloneable
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
        public float MinLineHeight { get; set; }
        public float VerticalSpaceBetweenLines { get; set; }
        public Line Outline { get; set; }

        public class Line : ICloneable
        {
            public Color color = Color.Black;
            public float thickness = 1f;

            public Line(Color color, float thickness)
            {
                this.color = color;
                this.thickness = thickness;
            }

            public Line CloneLine() => (Line)this.MemberwiseClone();
            public object Clone() => CloneLine();
        }

        public MarkupSettings(MyFont myFont, Anchor anchor, Color? textColor = null, float align = 0.5f, Vector2? scale = null, float rotation = 0f, SpriteEffects effects = SpriteEffects.None)
        {
            MyFont = myFont;
            Anchor = anchor;
            TextColor = textColor ?? Color.Black;
            HorizontalAlignInLine = align;
            Scale = scale ?? Vector2.One;
            Rotation = rotation;
            Effects = effects;
        }

        public MarkupSettings CloneDrawSettings()
        {
            MarkupSettings clone = (MarkupSettings)this.MemberwiseClone();
            clone.Anchor = Anchor?.Clone();
            clone.Outline = Outline?.CloneLine();
            return clone;
        }
        public object Clone() => CloneDrawSettings();
    }
}
