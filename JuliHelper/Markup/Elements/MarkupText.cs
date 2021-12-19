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
            if (settings.TextOutline != null
                && settings.TextOutline.SizeUnion)
            {
                size.X += settings.TextOutline.Thickness * 2f * settings.Scale.X;
                // modifying size.Y would mess with vertical positioning (if top aligned f.ex.)
            }
            return size;
        }

        protected override void DrawChild(MarkupSettings settings)
        {
            if (settings.TextUnderline == null)
                DrawJau(settings);
            else
                Drawer.TextUnderline(settings.TextUnderline.Color, settings.TextUnderline.Thickness, settings.TextUnderline.Offset, () => DrawJau(settings));
        }

        private void DrawJau(MarkupSettings settings)
        {
            if (settings.TextOutline == null)
            {
                DrawChildInner(settings);
            }
            else
            {
                if (settings.TextOutline.SizeUnion)
                {
                    float xTemp = settings.Anchor.X;
                    settings.Anchor.X += settings.TextOutline.Thickness * settings.Scale.X;
                    DrawChildInnerOutline(settings);
                    settings.Anchor.X = xTemp;
                }
                else
                {
                    DrawChildInnerOutline(settings);
                }
                void DrawChildInnerOutline(MarkupSettings settings)
                {
                    Drawer.TextOutline(settings.TextOutline.Color, settings.TextOutline.Thickness * settings.Scale.Average(), settings.TextOutline.Quality, () =>
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
