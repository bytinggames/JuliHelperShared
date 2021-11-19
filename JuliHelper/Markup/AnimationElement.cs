using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuliHelper.Markup
{
    [CreatorShortcut("anim")]
    public class AnimationElement : TextureElement
    {
        private readonly AnimationData animationData;
        private readonly AnimationData.Meta.FrameTag frameTag;

        // play the whole animation
        public AnimationElement(ContentManager content, string texName) : base(content, texName)
        {
            animationData = AnimationData.GetAnimationData(content, "Textures/" + texName);
            frameTag = null;
        }

        public AnimationElement(ContentManager content, string texName, string animationTagName) : base(content, texName)
        {
            animationData = AnimationData.GetAnimationData(content, "Textures/" + texName);
            frameTag = animationData.GetFrameTag(animationTagName);
        }

        protected override Vector2 GetSizeChild(DrawSettings settings)
        {
            return animationData.GetSourceRectangle(settings.Time, frameTag).Size.ToVector2();
        }

        protected override void DrawChild(DrawSettings settings)
        {
            SourceRectangle = animationData.GetSourceRectangle(settings.Time, frameTag);
            base.DrawChild(settings);
        }
    }
}
