using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuliHelper.Markup
{
    [CreatorShortcut("tex")]
    public class TextureElement : ILeaf
    {
        public Rectangle BoundingRectangle { get; }

        public Texture2D Texture { get; }

        public Color Color { get; set; } = Color.White;
        public Rectangle? SourceRectangle { get; set; } = null;

        public TextureElement(ContentManager content, string tex)
        {
            Texture = content.Load<Texture2D>("Textures/" + tex);
        }

        public Vector2 GetSize(DrawSettings settings)
        {
            if (SourceRectangle == null)
                return Texture.GetSize();
            else
                return SourceRectangle.Value.Size.ToVector2();
        }

        public void Draw(DrawSettings settings)
        {
            Texture.Draw(settings.Anchor, Color, SourceRectangle, settings.Scale, settings.Rotation, settings.Effects);
        }

        public override string ToString()
        {
            return "tex: " + Texture.Name;
        }
    }
}
