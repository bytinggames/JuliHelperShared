using Microsoft.Xna.Framework;

namespace JuliHelper.Markup
{
    public class NewLineElement : ILeaf
    {
        public void Draw(DrawSettings settings) { }

        public Vector2 GetSize(DrawSettings settings)
        {
            return new Vector2(0, settings.MyFont.Font.LineSpacing);
        }
    }
}
