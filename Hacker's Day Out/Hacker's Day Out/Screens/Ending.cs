﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using HackersDayOut.Screens;
using HackersDayOut.StateManagement;
using static System.TimeZoneInfo;

namespace HackersDayOut.Screens
{
    public class Ending : GameScreen
    {
        private ContentManager _content;
        private Texture2D _backgroundTexture;
        

        private bool _highScoreReached = false;

        public Ending()
        {

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            base.Activate();
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
           
            _backgroundTexture = _content.Load<Texture2D>("Sprite_ending");
            

        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ScreenManager.GameWon = true;
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(_backgroundTexture, fullscreen,
                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            

           





            spriteBatch.End();
        }
    }
}