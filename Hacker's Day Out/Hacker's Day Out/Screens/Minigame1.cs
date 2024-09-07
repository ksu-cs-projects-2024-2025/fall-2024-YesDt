using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackersDayOut.StateManagement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using SharpDX.Direct2D1.Effects;
using System.Reflection.Metadata;

namespace HackersDayOut.Screens
{
   
    public class Minigame1: GameScreen
    {
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;


        private ContentManager _content;

        private SpriteFont _gameFont;


        public Minigame1()
        {

        }

        public override void Activate()
        {
            _graphics = ScreenManager.Game.GraphicsDevice;
            _spriteBatch = ScreenManager.SpriteBatch;



            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("Title");
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // TODO: Add your update logic here

        }

        public override void Draw(GameTime gameTime)
        {


            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_gameFont, "Hello World", new Vector2(300, 150), Color.Black);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
