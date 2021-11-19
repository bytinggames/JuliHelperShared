﻿using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace JuliHelper.Markup
{
    public class ElementBlock
    {
        ElementCollection Root { get; }

        public ElementBlock(Creator creator, string text)
        {
            Root = new ElementCollection(creator, text);
        }

        public void Draw(DrawSettings _settings)
        {
            (Vector2 totalSize, Vector2[] lineSizes) = GetSizes(_settings);
            Vector2 topLeft = _settings.Anchor.Rectangle(totalSize.X, totalSize.Y).TopLeft;
            Vector2 topLeftOfLine = topLeft;
            DrawSettings settings = _settings.CloneDrawSettings(); // clone to modify the anchor

            int lineIndex = 0;

            foreach (var line in GetLinesOfLeaves(settings))
            {
                Vector2 lineSize = lineSizes[lineIndex];
                float emptyHorizontalSpace = totalSize.X - lineSize.X;
                M_Rectangle lineBounds = new M_Rectangle(topLeftOfLine.X + settings.HorizontalAlignInLine * emptyHorizontalSpace, topLeftOfLine.Y, lineSize.X, lineSize.Y);
                settings.Anchor = new Anchor(lineBounds.X, lineBounds.Y + settings.VerticalAlignInLine * lineSize.Y, 0, settings.VerticalAlignInLine);
                foreach (var element in line)
                {
                    element.Draw(settings);
                    settings.Anchor.X += element.GetSize(settings).X;
                }
                topLeftOfLine.X = topLeft.X;
                topLeftOfLine.Y += lineSize.Y;

                lineIndex++;
            }
        }

        public Vector2 GetSize(DrawSettings settings)
        {
            Vector2 totalSize = new Vector2();
            foreach (var size in GetLinesSizes(settings))
            {
                totalSize.Y += size.Y;
                if (size.X > totalSize.X)
                    totalSize.X = size.X;
            }
            return totalSize;
        }

        public (Vector2 totalSize, Vector2[] lineSizes) GetSizes(DrawSettings settings)
        {
            Vector2 totalSize = new Vector2();
            Vector2[] lineSizes = GetLinesSizes(settings).ToArray();
            foreach (var size in lineSizes)
            {
                totalSize.Y += size.Y;
                if (size.X > totalSize.X)
                    totalSize.X = size.X;
            }
            return (totalSize, lineSizes);
        }

        public class LineWithSize
        {
            public Vector2 size;
            public IEnumerable<INode> elements;

            public LineWithSize(Vector2 size, IEnumerable<INode> elements)
            {
                this.size = size;
                this.elements = elements;
            }
        }

        public IEnumerable<Vector2> GetLinesSizes(DrawSettings settings)
        {
            foreach (var line in GetLinesOfLeaves(settings))
            {
                Vector2 lineSize = Vector2.Zero;
                foreach (var element in line)
                {
                    Vector2 size = element.GetSize(settings);
                    lineSize.X += size.X;
                    if (size.Y > lineSize.Y)
                        lineSize.Y = size.Y;
                }
                yield return lineSize;
            }
        }

        public IEnumerable<IEnumerable<ILeaf>> GetLinesOfLeaves(DrawSettings settings)
        {
            IEnumerator<ILeaf> enumerator = Root.IterateOverLeaves(settings).GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                yield return GetNextLineOfLeaves(enumerator);
            }
        }

        public IEnumerable<ILeaf> GetNextLineOfLeaves(IEnumerator<ILeaf> enumerator)
        {
            do
            {
                if (enumerator.Current is NewLineElement)
                    yield break;
                yield return enumerator.Current;
            }
            while (enumerator.MoveNext());
        }

        public override string ToString()
        {
            return string.Join(" ", Root.Children.Select(f => f.ToString()));
        }
    }
}
