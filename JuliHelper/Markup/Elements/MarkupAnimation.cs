using JuliHelper.Creation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuliHelper.Markup
{
    [CreatorShortcut("anim")]
    public class MarkupAnimation : MarkupTexture
    {
        private readonly AnimationData animationData;
        private readonly AnimationData.Meta.FrameTag frameTag;

        // play the whole animation
        public MarkupAnimation(ContentManager content, string texName) : base(content, texName)
        {
            animationData = AnimationData.GetAnimationData(content, "Textures/" + texName);
            frameTag = null;
        }

        public MarkupAnimation(ContentManager content, string texName, string animationTagName) : base(content, texName)
        {
            animationData = AnimationData.GetAnimationData(content, "Textures/" + texName);
            frameTag = animationData.GetFrameTag(animationTagName);
        }

        protected override Vector2 GetSizeChild(MarkupSettings settings)
        {
            return animationData.GetSourceRectangle(settings.Time, frameTag).Size.ToVector2();
        }

        protected override void DrawChild(MarkupSettings settings)
        {
            SourceRectangle = animationData.GetSourceRectangle(settings.Time, frameTag);
            base.DrawChild(settings);
        }
    }
}
