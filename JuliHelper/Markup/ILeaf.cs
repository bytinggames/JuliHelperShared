using Microsoft.Xna.Framework;

namespace JuliHelper.Markup
{
    /// <summary>Elements that are rendered.</summary>
    public interface ILeaf : INode
    {
        void Draw(DrawSettings settings);
        Vector2 GetSize(DrawSettings settings);
    }
}
