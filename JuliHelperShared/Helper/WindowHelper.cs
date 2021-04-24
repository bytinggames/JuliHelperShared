using JuliHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelperShared
{
    public class WindowHelper
    {
        GraphicsDeviceManager graphics;
        GameWindow window;

        int screenWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int screenHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int centerX => window.Position.X + window.ClientBounds.Width / 2;
        int centerY => window.Position.Y + window.ClientBounds.Height / 2;

        int rememberWindowWidth, rememberWindowHeight;

        public WindowHelper(GraphicsDeviceManager graphics, GameWindow window)
        {
            this.graphics = graphics;
            this.window = window;
        }


        public void ToggleFullscreen()
        {
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = rememberWindowWidth;
                graphics.PreferredBackBufferHeight = rememberWindowHeight;
                graphics.ApplyChanges();

                Point pos = new Point((screenWidth - rememberWindowWidth) / 2, (screenHeight - rememberWindowHeight) / 2);
                pos.X += centerX / screenWidth * screenWidth;
                window.Position = pos;
            }
            else
            {
                rememberWindowWidth = graphics.PreferredBackBufferWidth;
                rememberWindowHeight = graphics.PreferredBackBufferHeight;

                graphics.PreferredBackBufferWidth = screenWidth;
                graphics.PreferredBackBufferWidth = screenHeight;
                //graphics.IsFullScreen = true;
                graphics.ApplyChanges();

                //Point pos = new Point(0, 0);
                //pos.X += centerX / screenWidth * screenWidth;
                //window.Position = pos;
            }

        }

        public void SwapScreen()
        {
            if (centerX < screenWidth)
                window.Position = new Point(window.Position.X + screenWidth, window.Position.Y);
            else
                window.Position = new Point(window.Position.X - screenWidth, window.Position.Y);
        }

        public void UpdateBasicInputFunctions()
        {
            if (Input.f11.pressed)
                ToggleFullscreen();
            if (Input.right.pressed || Input.left.pressed)
                SwapScreen();
        }
    }
}
