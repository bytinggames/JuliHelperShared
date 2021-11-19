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

        public override Vector2 GetSize(DrawSettings settings)
        {
            return animationData.GetSourceRectangle(settings.Time, frameTag).Size.ToVector2();
        }

        public override void Draw(DrawSettings settings)
        {
            SourceRectangle = animationData.GetSourceRectangle(settings.Time, frameTag);
            base.Draw(settings);
        }
    }
}
