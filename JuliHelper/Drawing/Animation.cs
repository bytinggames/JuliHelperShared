using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace JuliHelper
{
    public class Animation
    {
        private readonly Texture2D texture;
        private readonly AnimationData data;

        public Animation(Texture2D texture, AnimationData data)
        {
            this.texture = texture;
            this.data = data;
        }
    }
}
