using System.Collections.Generic;

namespace JuliHelper.Markup
{
    /// <summary>Elements that contain other elements. May modify the draw settings.</summary>
    public interface IBranch : INode
    {
        IEnumerable<ILeaf> IterateOverLeaves(DrawSettings settings);
    }
}
