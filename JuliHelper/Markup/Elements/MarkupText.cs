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
            Vector2 size = settings.MyFont.Font.MeasureString(Text).CeilVector();
            if (settings.Outline != null)
            {
                size.X += settings.Outline.thickness * 2f;
                // modifying size.Y would mess with vertical positioning (if top aligned f.ex.)
            }
            return size;
        }

        protected override void DrawChild(MarkupSettings settings)
        {
            if (settings.Outline == null)
            {
                DrawChildInner(settings);
            }
            else
            {
                float xTemp = settings.Anchor.X;
                settings.Anchor.X += settings.Outline.thickness;
                Drawer.TextOutline(settings.Outline.color, settings.Outline.thickness, () =>
                {
                    DrawChildInner(settings);
                });
                settings.Anchor.X = xTemp;
            }
        }

        protected void DrawChildInner(MarkupSettings settings)
        {
            settings.MyFont.Font.Draw(Text, settings.Anchor, settings.TextColor, settings.Scale, settings.Rotation, settings.Effects);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
