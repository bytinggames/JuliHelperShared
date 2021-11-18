using JuliHelper.Creating;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelper
{
    public class CodedTextException : Exception
    {
        public CodedTextException(string message) : base(message) { }
    }

    public class CodedText
    {
        public static DrawElementCollection GetElements(string text)
        {
            return new DrawElementCollection(text);
        }
    }

    public interface IDrawElement
    {
        //Rectangle BoundingRectangle { get; }
        //void Draw(SpriteBatch spriteBatch, Anchor anchor);
    }

    public class TextElement : IDrawElement
    {
        //public Rectangle BoundingRectangle { get; }
        public string Text { get; }

        public TextElement(ScriptReader reader)
        {
            Text = reader.ReadToCharOrEnd(out char? until, '#');
            if (until != null)
                reader.Move(-1);
        }

        public void Draw(SpriteBatch spriteBatch, Anchor anchor, MyFont myFont)
        {
            
            throw new NotImplementedException();
        }
    }

    public class TextureElement : IDrawElement
    {
        public Rectangle BoundingRectangle { get; }

        public TextureElement(ScriptReader reader)
        {
            string parameters = reader.ReadToCharOrEndConsiderOpenCloseBraces('>', '<', '>');
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CreatorShortcutAttribute : Attribute
    {
        public readonly string ShortcutName;

        public CreatorShortcutAttribute(string shortcutName)
        {
            this.ShortcutName = shortcutName;
        }
    }

    [CreatorShortcut("c")]
    public class ColorElement : DrawElementCollection
    {
        Color color;

        public ColorElement(string hexColor, string text)
            :base(text)
        {
            color = Calculate.HexToColor(hexColor);
        }
    }

    public class DrawElementCollection : IDrawElement
    {
        private static Creator creator = new Creator();

        public List<IDrawElement> Children { get; } = new List<IDrawElement>();

        public DrawElementCollection(string text)
        {
            ScriptReader reader = new ScriptReader(text);

            IDrawElement element;
            while ((element = ReadElement(reader)) != null)
            {
                Children.Add(element);
            }
        }

        private static IDrawElement ReadElement(ScriptReader reader)
        {
            char? peek = reader.Peek();
            if (!peek.HasValue)
                return null;

            if (peek != '#')
            {
                return new TextElement(reader);
            }
            else
            {
                reader.ReadChar(); // read in '#'

                return creator.CreateObject<IDrawElement>(reader);
            }
        }
    }
}
