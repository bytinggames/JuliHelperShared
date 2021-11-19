using JuliHelper.Creation;
using System;
using System.Collections.Generic;

namespace JuliHelper.Markup
{
    public class ElementCollection : IBranch
    {
        public List<INode> Children { get; } = new List<INode>();

        public virtual IEnumerable<ILeaf> IterateOverLeaves(DrawSettings settings)
        {
            foreach (var child in Children)
            {
                if (child is ILeaf leaf)
                    yield return leaf;
                else if (child is IBranch branch)
                {
                    foreach (var collectionChild in branch.IterateOverLeaves(settings))
                        yield return collectionChild;
                }
                else
                    throw new Exception(child.GetType() + " node must be either leaf or branch");
            }
        }

        public ElementCollection(Creator creator, string text)
        {
            ScriptReader reader = new ScriptReader(text);

            INode element;
            while ((element = ReadElement(creator, reader)) != null)
            {
                Children.Add(element);
            }
        }

        private static INode ReadElement(Creator creator, ScriptReader reader)
        {
            char? peek = reader.Peek();
            if (!peek.HasValue)
                return null;

            switch (peek)
            {
                case '#':
                    reader.ReadChar(); // read in '#'
                    return creator.CreateObject<INode>(reader);

                case '\n':
                    reader.ReadChar(); // read in '\n'
                    return new NewLineElement();

                default:
                    return new TextElement(reader);
            }
        }
    }
}
