using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace JuliHelper.Markup
{
    [MarkupShortcut("underline")]
    public class MarkupUnderline : MarkupCollection
    {
        Color color;
        float thickness;
        float offset;

        public bool SizeUnion { get; set; }

        public MarkupUnderline(Creator creator, string hexColor, string text)
            : base(creator, text)
        {
            color = Calculate.HexToColor(hexColor);
            thickness = 1f;
        }
        public MarkupUnderline(Creator creator, string hexColor, float thickness, string text)
            : base(creator, text)
        {
            color = Calculate.HexToColor(hexColor);
            this.thickness = thickness;
        }
        public MarkupUnderline(Creator creator, string hexColor, float thickness, float offset, string text)
            : base(creator, text)
        {
            color = Calculate.HexToColor(hexColor);
            this.thickness = thickness;
            this.offset = offset;
        }

        public override string ToString()
        {
            return $"Underline #{color.ColorToHex()} {base.ToString()}";
        }

        public override IEnumerable<ILeaf> IterateOverLeaves(MarkupSettings settings)
        {
            var temp = settings.TextUnderline?.CloneUnderline();
            settings.TextUnderline = new MarkupSettings.Underline(color, thickness, SizeUnion, offset);

            foreach (var leaf in base.IterateOverLeaves(settings))
                yield return leaf;

            settings.TextUnderline = temp;
        }
    }
}
