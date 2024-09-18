﻿using HackersDayOut.StateManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.TimeZoneInfo;

namespace HackersDayOut.Screens
{
    // The loading screen coordinates transitions between the menu system and the
    // game itself. Normally one screen will transition off at the same time as
    // the next screen is transitioning on, but for larger transitions that can
    // take a longer time to load their data, we want the menu system to be entirely
    // gone before we start loading the game. This is done as follows:
    // 
    // - Tell all the existing screens to transition off.
    // - Activate a loading screen, which will transition on at the same time.
    // - The loading screen watches the state of the previous screens.
    // - When it sees they have finished transitioning off, it activates the real
    //   next screen, which may take a long time to load its data. The loading
    //   screen will be the only thing displayed while this load is taking place.
    public class LoadingScreen : GameScreen
    {
        private ContentManager _content;
        private Texture2D _backgroundTexture;

        private readonly bool _loadingIsSlow;
        private bool _otherScreensAreGone;
        private readonly GameScreen[] _screensToLoad;



        // Constructor is private: loading screens should be activated via the static Load method instead.
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, GameScreen[] screensToLoad)
        {
            _loadingIsSlow = loadingIsSlow;
            _screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);


        }

        // Activates the loading screen.
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer, params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            var loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, screensToLoad);

            screenManager.AddScreen(loadingScreen, controllingPlayer);


        }

        public override void Activate()
        {
            base.Activate();
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _backgroundTexture = _content.Load<Texture2D>("blank");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);



            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (_otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (var screen in _screensToLoad)
                {
                    if (screen != null)
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Draw(GameTime gameTime)
        {

            if (ScreenState == ScreenState.Active && ScreenManager.GetScreens().Length == 1)
                _otherScreensAreGone = true;


            if (_loadingIsSlow)
            {
                var spriteBatch = ScreenManager.SpriteBatch;
                var font = ScreenManager.Font;



                const string message = "Loading...";

                // Center the text in the viewport.
                var viewport = ScreenManager.GraphicsDevice.Viewport;
                var viewportSize = new Vector2(viewport.Width, viewport.Height);
                var textSize = font.MeasureString(message);
                var textPosition = (viewportSize - textSize) / 2;

                var color = Color.White * TransitionAlpha;

                // Draw the text.
                spriteBatch.Begin();
                spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null,
                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha), 0f, new Vector2(0, 0), 1.09f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
            else
            {
                var spriteBatch = ScreenManager.SpriteBatch;
                var font = ScreenManager.Font;

                spriteBatch.Begin();
                spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null,
                new Color(0, 0, 0), 0f, new Vector2(0, 0), 1.09f, SpriteEffects.None, 0f);

                spriteBatch.End();
            }
        }
    }
}
