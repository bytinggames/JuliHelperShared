using Microsoft.Xna.Framework;

namespace JuliHelper.Markup
{
    public class TextElement : BlockLeaf
    {
        public string Text { get; }

        public TextElement(ScriptReader reader)
        {
            Text = reader.ReadToCharOrEnd(out char? until, '#', '\n');
            if (until != null)
                reader.Move(-1);
        }

        protected override Vector2 GetSizeChild(DrawSettings settings)
        {
            return settings.MyFont.Font.MeasureString(Text).CeilVector();
        }

        protected override void DrawChild(DrawSettings settings)
        {
            settings.MyFont.Font.Draw(Text, settings.Anchor, settings.TextColor, settings.Scale, settings.Rotation, settings.Effects);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
