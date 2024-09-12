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
    public class Minigame2 : GameScreen
    {
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        private Texture2D _screen;
        private Texture2D _success;

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


            RandomNum = random.Next(1, 3);
            if (RandomNum == 1)
            {
                MiniGameProblem = "numlist = [1, 2, 3, 4, 5, 6] \n" +
                    "Print('Divisible by 2:') + \n" +
                    "for num in numlist: \n" +
                    "   if num _ _ == 0 \n" +
                    "       print(num)";
            }
            else
            {
                MiniGameProblem = "num1 = 50 \n" +
                    "num2 = 19 \n" +
                    "result = _ _ _ \n" +
                    "Print('50 + 19 =', result)";

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
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
