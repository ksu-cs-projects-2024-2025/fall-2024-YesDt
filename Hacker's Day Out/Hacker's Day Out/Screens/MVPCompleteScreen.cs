using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackersDayOut.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackersDayOut.Screens
{
    public class MVPCompleteScreen: GameScreen
    {
        private ContentManager _content;
        private Texture2D _backgroundTextureOne;

        public MVPCompleteScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            base.Activate();
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _backgroundTextureOne = _content.Load<Texture2D>("MVPComplete");

        
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
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            //if (ScreenManager.TotalCoinsCollected <= 38) spriteBatch.Draw(_backgroundTextureOne, fullscreen,
            //    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            //else if (ScreenManager.TotalCoinsCollected <= 44) spriteBatch.Draw(_backgroundTextureTwo, fullscreen,
            //    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            //else if (ScreenManager.TotalCoinsCollected < 52) spriteBatch.Draw(_backgroundTextureThree, fullscreen,
            //    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            //else spriteBatch.Draw(_backgroundTextureFour, fullscreen,
            //    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
           
           spriteBatch.Draw(_backgroundTextureOne, fullscreen,
                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));


            spriteBatch.End();
        }
    }
}