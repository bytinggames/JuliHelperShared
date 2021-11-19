using JuliHelper.Creation;

namespace JuliHelper.Markup
{
    [MarkupShortcut("span")]
    public class MarkupSpan : MarkupText
    {
        public MarkupSpan(string str)
            : base(new ScriptReader(str))
        {
        }
    }
}
