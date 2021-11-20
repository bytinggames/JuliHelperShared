using Microsoft.Xna.Framework;

namespace JuliHelper.Markup
{
    public class MarkupNewLine : ILeaf
    {
        public void Draw(MarkupSettings settings) { }

        public Vector2 GetSize(MarkupSettings settings)
        {
            return new Vector2(0, settings.MyFont.Font.LineSpacing * settings.Scale.X);
        }

        public override string ToString()
        {
            return "\\n";
        }
    }
}
