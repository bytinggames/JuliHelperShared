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

        protected override Vector2 GetSizeChildUnscaled(MarkupSettings settings)
        {
            Vector2 size = settings.MyFont.Font.MeasureString(Text).CeilVector();
            if (settings.Outline != null
                && settings.Outline.SizeUnion)
            {
                size.X += settings.Outline.Thickness * 2f * settings.Scale.X;
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
                if (settings.Outline.SizeUnion)
                {
                    float xTemp = settings.Anchor.X;
                    settings.Anchor.X += settings.Outline.Thickness * settings.Scale.X;
                    DrawChildInnerOutline(settings);
                    settings.Anchor.X = xTemp;
                }
                else
                {
                    DrawChildInnerOutline(settings);
                }
                void DrawChildInnerOutline(MarkupSettings settings)
                {
                    Drawer.TextOutline(settings.Outline.Color, settings.Outline.Thickness * settings.Scale.Average(), () =>
                    {
                        DrawChildInner(settings);
                    });
                }
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
