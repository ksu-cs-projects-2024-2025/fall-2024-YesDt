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
//using SharpDX.Direct2D1;
using SharpDX.Direct3D9;

namespace HackersDayOut.Screens
{
    public class Minigame2 : GameScreen
    {


        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        private Texture2D _screen;
        private Texture2D _success;
        private Texture2D _button;

        private SpriteFont _objective;
        private SpriteFont _problem;
        private SpriteFont _answer;

       

        private Random random = new Random();

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState  _currentMouseState;
        private MouseState _previousMouse;

        private bool _isHovering;

        private double _successTimer;

        private Button[] _buttons1;

        private int _selectedButtonIndex = 0;

        public string MiniGameProblem;

        public string Answer = " ";

        public bool succeeded;

        public int RandomNum;

        public Minigame2()
        {

        }
        public override void Activate()
        {
            _graphics = ScreenManager.Game.GraphicsDevice;
            _spriteBatch = ScreenManager.SpriteBatch;
            succeeded = false;
            _successTimer = 0;

            
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _screen = _content.Load<Texture2D>("MinigameDoorScreen");
            _success = _content.Load<Texture2D>("Sprite_success");
            _objective = _content.Load<SpriteFont>("MG1objective");
            _problem = _content.Load<SpriteFont>("MG1problem");
            _answer = _content.Load<SpriteFont>("MG1answer");

            _buttons1 = new Button[]
             {
                 new Button(new Vector2(100, 500)),
                 new Button(new Vector2(50, 550)),
                 new Button(new Vector2(100, 600)),
                 new Button(new Vector2(150, 550))
             };
            foreach (var b in _buttons1) b.LoadContent(_content);

            RandomNum = random.Next(1, 3);
            if (RandomNum == 1)
            {
                MiniGameProblem = "The hypothetical computing machine was made in 1936 by\n" +
                    "Alan _________ ";
            }
            else
            {
                MiniGameProblem = "The fastest algorithm to sort in Python is \n" +
                    "_________ sort";

            }

        }
        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
           

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_screen, new Rectangle(0, 0, 800, 500), Color.White);
            _spriteBatch.DrawString(_objective, "Find the correct word", new Vector2(50, 150), Color.Red);
            _spriteBatch.DrawString(_problem, MiniGameProblem, new Vector2(200, 250), Color.Black);
            foreach(var b in _buttons1)
            {
                b.Draw(gameTime, _spriteBatch);
            }
            //_spriteBatch.Draw(_button, new Vector2(100, 500), source, Color.White, 0f, new Vector2(116, 70), 0f, SpriteEffects.None, 0);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
