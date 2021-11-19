using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelper.Markup
{
    public abstract class BlockLeaf : ILeaf
    {
        public float MarginRight { get; set; }
        public float MarginLeft { get; set; }
        public float MarginTop { get; set; }
        public float MarginBottom { get; set; }

        private ElementContainer subContainer;

        public bool SubSizeUnion { get; set; } = false;

        public float SubAnchorX { get; set; } = 0.5f;
        /// <summary>No SubAnchorY support for SubSizeUnion yet.</summary>
        public float SubAnchorY { get; set; } = 0.5f;

        public void Sub(Creator creator, string text)
        {
            subContainer = new ElementContainer(creator, text);
        }

        public void SubAnchor(float x, float y)
        {
            SubAnchorX = x;
            SubAnchorY = y;
        }


        public void Draw(DrawSettings settings)
        {
            if (MarginLeft == 0 && MarginRight == 0)
                InnerDraw(settings);
            else
            {
                Vector2 temp = settings.Anchor.pos;
                settings.Anchor.pos.X += MarginLeft;
                settings.Anchor.pos.Y += MarginTop;
                InnerDraw(settings);
                settings.Anchor.pos = temp;
            }
        }
        private void InnerDraw(DrawSettings settings)
        {
            float tempX = settings.Anchor.X;
            if (SubSizeUnion && subContainer != null)
            {
                Vector2 thisSize = GetSizeChild(settings);
                Vector2 subSize = subContainer.GetSize(settings);
                Vector2 larger = subSize - thisSize;
                if (larger.X > 0)
                    settings.Anchor.X += larger.X * SubAnchorX; // TODO: depends on anchor
            }
            DrawChild(settings);
            if (subContainer != null)
            {
                var settingsClone = settings.CloneDrawSettings();
                settingsClone.Anchor = settings.Anchor.Rectangle(GetSizeChild(settingsClone)).GetAnchor(SubAnchorX, SubAnchorY);
                subContainer.Draw(settingsClone);
            }

            settings.Anchor.X = tempX;
        }

        protected abstract void DrawChild(DrawSettings settings);

        public Vector2 GetSize(DrawSettings settings)
        {
            Vector2 size = GetSizeChild(settings);

            if (SubSizeUnion && subContainer != null)
            {
                Vector2 subSize = subContainer.GetSize(settings);
                size = Vector2.Max(size, subSize);
            }

            size.X += MarginRight + MarginLeft;
            size.Y += MarginTop + MarginBottom;
            return size;
        }

        protected abstract Vector2 GetSizeChild(DrawSettings settings);
    }
}
