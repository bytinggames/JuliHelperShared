using Microsoft.Xna.Framework;

namespace JuliHelper.Markup
{
    public class MarkupText : MarkupBlock
    {
        public string Text { get; }

        public MarkupText(ScriptReader reader)
        {
            Text = reader.ReadToCharOrEnd(out char? until, '#', '\n');
            if (until != null)
                reader.Move(-1);
        }

        protected override Vector2 GetSizeChild(MarkupSettings settings)
        {
            return settings.MyFont.Font.MeasureString(Text).CeilVector();
        }

        protected override void DrawChild(MarkupSettings settings)
        {
            settings.MyFont.Font.Draw(Text, settings.Anchor, settings.TextColor, settings.Scale, settings.Rotation, settings.Effects);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
