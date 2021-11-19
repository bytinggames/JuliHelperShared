﻿using System;

namespace JuliHelper.Creation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class MarkupShortcutAttribute : CreatorShortcutAttribute
    {
        public MarkupShortcutAttribute(string shortcutName)
            :base(shortcutName)
        {
        }
    }
}
