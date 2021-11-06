using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace JuliHelper
{
    public class Animation
    {
        public Texture2D Texture { get; }
        public AnimationData Data { get; }

        public Animation(Texture2D texture, AnimationData data)
        {
            this.Texture = texture;
            this.Data = data;
        }
    }
}
