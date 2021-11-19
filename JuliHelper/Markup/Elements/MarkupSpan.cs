using JuliHelper.Creation;

namespace JuliHelper.Markup
{
    [CreatorShortcut("span")]
    public class MarkupSpan : MarkupText
    {
        public MarkupSpan(string str)
            : base(new ScriptReader(str))
        {
        }
    }
}
