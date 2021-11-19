using Microsoft.Xna.Framework;

namespace JuliHelper.Markup
{
    public class TextElement : ILeaf
    {
        public string Text { get; }

        public TextElement(ScriptReader reader)
        {
            Text = reader.ReadToCharOrEnd(out char? until, '#', '\n');
            if (until != null)
                reader.Move(-1);
        }

        public Vector2 GetSize(DrawSettings settings)
        {
            return settings.MyFont.Font.MeasureString(Text).CeilVector();
        }

        public void Draw(DrawSettings settings)
        {
            settings.MyFont.Font.Draw(Text, settings.Anchor, settings.TextColor, settings.Scale, settings.Rotation, settings.Effects);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
