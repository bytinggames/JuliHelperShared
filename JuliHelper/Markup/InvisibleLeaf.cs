using JuliHelper.Creation;
using Microsoft.Xna.Framework;

namespace JuliHelper.Markup
{
    public class InvisibleLeaf : ILeaf
    {
        public void Draw(DrawSettings settings) { }

        public Vector2 GetSize(DrawSettings settings) => Vector2.Zero;
    }
}
