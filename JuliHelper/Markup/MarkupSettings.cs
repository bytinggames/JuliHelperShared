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
        public Color TextureColor { get; set; } = Color.White;
        public float HorizontalAlignInLine { get; set; }
        public float VerticalAlignInLine { get; set; } = 0.5f;
        public Vector2 Scale { get; set; }
        public float Rotation { get; }
        public SpriteEffects Effects { get; }
        public long Time { get; set; }
        public float MinLineHeight { get; set; }
        public float VerticalSpaceBetweenLines { get; set; }
        public Outline TextOutline { get; set; }
        public Underline TextUnderline { get; set; }

        public class Line : ICloneable
        {
            public Color Color = Color.Black;
            public float Thickness = 1f;
            /// <summary>Wether the outline is considered when calculating the positioning (only horizontally).</summary>
            public bool SizeUnion = true;

            public Line(Color color, float thickness, bool sizeUnion)
            {
                this.Color = color;
                this.Thickness = thickness;
                this.SizeUnion = sizeUnion;
            }

            public Line CloneLine() => (Line)this.MemberwiseClone();
            public object Clone() => CloneLine();
        }

        public class Outline : Line
        {
            public int Quality;

            public Outline(Color color, float thickness, bool sizeUnion, int quality) : base(color, thickness, sizeUnion)
            {
                this.Quality = quality;
            }

            public Outline CloneOutline() => (Outline)this.Clone();
        }

        public class Underline : Line
        {
            public float Offset;

            public Underline(Color color, float thickness, bool sizeUnion, float offset) : base(color, thickness, sizeUnion)
            {
                this.Offset = offset;
            }

            public Underline CloneUnderline() => (Underline)this.Clone();
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

        public MarkupSettings CloneMarkupSettings()
        {
            MarkupSettings clone = (MarkupSettings)this.MemberwiseClone();
            clone.Anchor = Anchor?.Clone();
            clone.TextOutline = TextOutline?.CloneOutline();
            clone.TextUnderline = TextUnderline?.CloneUnderline();
            return clone;
        }
        public object Clone() => CloneMarkupSettings();
    }
}
