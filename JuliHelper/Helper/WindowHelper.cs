﻿using JuliHelper;
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

        int rememberWindowWidth = 1920 / 2, rememberWindowHeight = 1080 / 2;
        bool rememberRealFullscreen = true; // wether graphics.IsFullScreen is used for fullscreen mode

        public WindowHelper(GraphicsDeviceManager graphics, GameWindow window)
        {
            this.graphics = graphics;
            this.window = window;
        }

        private bool IsFullScreenFilled()
        {
            return graphics.IsFullScreen
                || graphics.PreferredBackBufferWidth == screenWidth
                && graphics.PreferredBackBufferHeight == screenHeight;
        }

        public void ToggleFullscreen()
        {
            if (IsFullScreenFilled())
            {
                rememberRealFullscreen = graphics.IsFullScreen;

                graphics.PreferredBackBufferWidth = rememberWindowWidth;
                graphics.PreferredBackBufferHeight = rememberWindowHeight;
                graphics.IsFullScreen = false;
                graphics.ApplyChanges();

                window.IsBorderless = false;

                Point pos = new Point((screenWidth - rememberWindowWidth) / 2, (screenHeight - rememberWindowHeight) / 2);
                pos.X += centerX / screenWidth * screenWidth;
                window.Position = pos;
            }
            else
            {
                rememberWindowWidth = graphics.PreferredBackBufferWidth;
                rememberWindowHeight = graphics.PreferredBackBufferHeight;

                window.IsBorderless = true;

                graphics.PreferredBackBufferWidth = screenWidth;
                graphics.PreferredBackBufferHeight = screenHeight;
                graphics.IsFullScreen = rememberRealFullscreen;
                graphics.ApplyChanges();

                if (!graphics.IsFullScreen)
                {
                    Point pos = new Point(0, 0);
                    pos.X += centerX / screenWidth * screenWidth;
                    window.Position = pos;
                }
            }

        }

        public void UpdateBasicInputFunctions()
        {
            if (Input.f11.pressed)
                ToggleFullscreen();
            if (Input.leftControl.down || Input.rightControl.down)
            {
                if (Input.left.pressed)
                    window.Position = new Point(window.Position.X - screenWidth, window.Position.Y);
                if (Input.right.pressed)
                    window.Position = new Point(window.Position.X + screenWidth, window.Position.Y);
                if (Input.down.pressed)
                    window.Position = new Point((screenWidth - graphics.PreferredBackBufferWidth) / 2, (screenHeight - graphics.PreferredBackBufferHeight) / 2);
            }
        }
    }
}
