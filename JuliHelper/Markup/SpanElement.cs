using JuliHelper.Creation;

namespace JuliHelper.Markup
{
    [CreatorShortcut("span")]
    public class SpanElement : TextElement
    {
        public SpanElement(string str)
            : base(new ScriptReader(str))
        {
        }
    }
}
