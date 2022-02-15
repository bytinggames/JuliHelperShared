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
        /// <summary>Negative for only bottom outline (used for drawing over the underline).</summary>
        int quality = 4;

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

        public MarkupOutline(Creator creator, string hexColor, float thickness, int quality, string text)
            : base(creator, text)
        {
            color = Calculate.HexToColor(hexColor);
            this.thickness = thickness;
            this.quality = quality;
        }

        public override string ToString()
        {
            return $"Outline #{color.ColorToHex()} {base.ToString()}";
        }

        public override IEnumerable<ILeaf> IterateOverLeaves(MarkupSettings settings)
        {
            var temp = settings.TextOutline?.CloneOutline();
            settings.TextOutline = new MarkupSettings.Outline(color, thickness, SizeUnion, quality);

            foreach (var leaf in base.IterateOverLeaves(settings))
                yield return leaf;

            settings.TextOutline = temp;
        }
    }
}
