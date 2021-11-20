using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace JuliHelper.Markup
{
    [MarkupShortcut("outline")]
    public class MarkupOutline : MarkupCollection
    {
        Color color;
        float thickness;

        public bool SizeUnion { get; set; }

        public MarkupOutline(Creator creator, string hexColor, string text)
            : base(creator, text)
        {
            color = Calculate.HexToColor(hexColor);
            thickness = 1f;
        }
        public MarkupOutline(Creator creator, string hexColor, float thickness, string text)
            : base(creator, text)
        {
            color = Calculate.HexToColor(hexColor);
            this.thickness = thickness;
        }

        public override string ToString()
        {
            return $"Outline #{color.ColorToHex()} {base.ToString()}";
        }

        public override IEnumerable<ILeaf> IterateOverLeaves(MarkupSettings settings)
        {
            var temp = settings.Outline?.CloneLine();
            settings.Outline = new MarkupSettings.Line(color, thickness, SizeUnion);

            foreach (var leaf in base.IterateOverLeaves(settings))
                yield return leaf;

            settings.Outline = temp;
        }
    }
}
