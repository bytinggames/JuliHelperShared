using System;
using System.Collections.Generic;
using System.Text;
using JuliHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuliHelperShared
{
    public class TextureAnim
    {
        public Texture2D Texture { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int FrameCount { get; set; }
        public float Fps { get; set; }

        public TextureAnim(Texture2D tex)
        {
            Initialize(tex);

        }
        public TextureAnim(Texture2D tex, string[] aniContent)
        {
            FrameCount = FrameHeight = FrameWidth = -1;

            for (int i = 0; i < aniContent.Length; i++)
            {
                string[] split = aniContent[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                switch (split[0])
                {
                    case "w":
                        FrameWidth = Convert.ToInt32(split[1]);
                        break;
                    case "h":
                        FrameHeight = Convert.ToInt32(split[1]);
                        break;
                    case "count":
                        FrameCount = Convert.ToInt32(split[1]);
                        break;
                    case "fps":
                        Fps = Convert.ToSingle(split[1]);
                        break;
                }
            }
            Initialize(tex, FrameWidth, FrameHeight, FrameCount);
        }

        public Rectangle GetFrameRect(int frame)
        {
            frame %= FrameCount;
            if (frame < 0)
                frame += FrameCount;

            int framesInX = Texture.Width / FrameWidth;

            int x = frame % framesInX;
            int y = frame / framesInX;

            return new Rectangle(x * FrameWidth, y * FrameHeight, FrameWidth, FrameHeight);
        }

        public TextureAnim(Texture2D tex, int frameWidth, int frameHeight)
        {
            Initialize(tex, frameWidth, frameHeight);
        }

        public TextureAnim(Texture2D tex, int frameWidth, int frameHeight, int frameCount)
        {

            Initialize(tex, frameWidth, frameHeight, frameCount);
        }

        private void Initialize(Texture2D _tex, int _frameWidth = -1, int _frameHeight = -1, int _frameCount = -1)
        {
            Texture = _tex;

            if (_frameWidth != -1)
                FrameWidth = _frameWidth;
            else
            {
                if (_frameCount != 1)
                {
                    FrameWidth = Texture.Height;

                    if (Texture.Width / FrameWidth * FrameWidth != Texture.Width)
                        throw new Exception($"Texture should be {Texture.Width / FrameWidth * FrameWidth} pixels wide, but is {Texture.Width} pixels wide");
                }
                else
                {
                    FrameWidth = Texture.Width;
                }
            }
            if (_frameHeight != -1)
                FrameHeight = _frameHeight;
            else
            {
                FrameHeight = Texture.Height;
            }

            if (_frameCount != -1)
                FrameCount = _frameCount;
            else
            {
                FrameCount = (Texture.Width / FrameWidth) * (Texture.Height / FrameHeight);
            }
        }

        public void CopySettingsTo(TextureAnim ani)
        {
            if (Texture != ani.Texture)
                ani.Texture.SetData(Texture.ToColor());
            ani.FrameWidth = FrameWidth;
            ani.FrameHeight = FrameHeight;
            ani.FrameCount = FrameCount;
            ani.Fps = Fps;
        }
    }

}
