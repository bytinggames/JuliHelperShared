using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelper.Markup
{
    public abstract class MarkupBlock : ILeaf
    {
        public float MarginRight { get; set; }
        public float MarginLeft { get; set; }
        public float MarginTop { get; set; }
        public float MarginBottom { get; set; }

        // for sub element
        /// <summary>No PaddingRight support for SubSizeUnion yet.</summary>
        public float PaddingRight { get; set; }
        /// <summary>No PaddingLeft support for SubSizeUnion yet.</summary>
        public float PaddingLeft { get; set; }
        /// <summary>No PaddingTop support for SubSizeUnion yet.</summary>
        public float PaddingTop { get; set; }
        /// <summary>No PaddingBottom support for SubSizeUnion yet.</summary>
        public float PaddingBottom { get; set; }

        private MarkupRoot subContainer;

        public bool SubSizeUnion { get; set; } = false;

        public float SubAnchorX { get; set; } = 0.5f;
        /// <summary>No SubAnchorY support for SubSizeUnion yet.</summary>
        public float SubAnchorY { get; set; } = 0.5f;

        public void Sub(Creator creator, string text)
        {
            subContainer = new MarkupRoot(creator, text);
        }

        public void SubAnchor(float x, float y)
        {
            SubAnchorX = x;
            SubAnchorY = y;
        }


        public void Draw(MarkupSettings settings)
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
        private void InnerDraw(MarkupSettings settings)
        {
            float tempX = settings.Anchor.X;
            if (SubSizeUnion && subContainer != null)
            {
                Vector2 thisSize = GetSizeChild(settings);
                Vector2 subSize = subContainer.GetSize(settings);
                Vector2 larger = subSize - thisSize;
                if (larger.X > 0)
                    settings.Anchor.X += larger.X * SubAnchorX;
            }
            DrawChild(settings);

            //settings.Anchor.Rectangle(GetSize(settings)).Draw(Color.Red * 0.5f);

            if (subContainer != null)
            {
                var settingsClone = settings.CloneDrawSettings();
                M_Rectangle ownRect = settings.Anchor.Rectangle(GetSizeChild(settingsClone));
                ownRect.ApplyPadding(PaddingLeft, PaddingRight, PaddingTop, PaddingBottom);
                settingsClone.Anchor = ownRect.GetAnchor(SubAnchorX, SubAnchorY);
                subContainer.Draw(settingsClone);
            }

            settings.Anchor.X = tempX;
        }

        protected abstract void DrawChild(MarkupSettings settings);

        public Vector2 GetSize(MarkupSettings settings)
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

        protected abstract Vector2 GetSizeChild(MarkupSettings settings);
    }
}
