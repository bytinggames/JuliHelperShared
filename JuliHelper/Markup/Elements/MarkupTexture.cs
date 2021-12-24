using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuliHelper.Markup
{
    [MarkupShortcut("tex")]
    public class MarkupTexture : MarkupBlock
    {
        public Rectangle BoundingRectangle { get; }

        public Texture2D Texture { get; }

        public Color Color { get; set; } = Color.White;
        public Rectangle? SourceRectangle { get; set; } = null;

        public Vector2 ScaleXY { get; set; } = Vector2.One;

        public float Scale
        {
            get => ScaleXY.X;
            set => ScaleXY = new Vector2(value);
        }

        public MarkupTexture(ContentManager content, string texName)
        {
            Texture = content.Load<Texture2D>("Textures/" + texName);
        }

        protected override Vector2 GetSizeChildUnscaled(MarkupSettings settings)
        {
            return GetSizeChildUnscaledInternal(settings) * ScaleXY;
        }

        private Vector2 GetSizeChildUnscaledInternal(MarkupSettings settings)
        {
            if (SourceRectangle == null)
                return Texture.GetSize();
            else
                return SourceRectangle.Value.Size.ToVector2();
        }

        protected override void DrawChild(MarkupSettings settings)
        {
            Texture.Draw(settings.Anchor, Calculate.MultiplyColors(settings.TextureColor, Color), SourceRectangle, settings.Scale * ScaleXY, settings.Rotation, settings.Effects);
        }

        public override string ToString()
        {
            return "tex: " + Texture.Name;
        }
    }
}
