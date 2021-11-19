using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace JuliHelper.Markup
{
    [MarkupShortcut("c")]
    public class MarkupColor : MarkupCollection
    {
        Color textColor;

        public MarkupColor(Creator creator, string hexColor, string text)
            :base(creator, text)
        {
            textColor = Calculate.HexToColor(hexColor);
        }

        public override string ToString()
        {
            return $"#{textColor.ColorToHex()} {base.ToString()}";
        }

        public override IEnumerable<ILeaf> IterateOverLeaves(MarkupSettings settings)
        {
            Color temp = settings.TextColor;
            settings.TextColor = textColor;

            foreach (var leaf in base.IterateOverLeaves(settings))
                yield return leaf;

            settings.TextColor = temp;
        }
    }
}
