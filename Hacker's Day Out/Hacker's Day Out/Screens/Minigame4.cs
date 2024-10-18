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
    public class Minigame4 : GameScreen
    {


        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        private Texture2D _screen;
        private Texture2D _success;
        private Texture2D _invalid;
        private Texture2D _bomb;
        private Texture2D _button;
        private Texture2D _debugCircle;

        private SpriteFont _objective;
        private SpriteFont _problem;
        private SpriteFont _answer1;
        private SpriteFont _answer2;


        private SpriteFont _choice1;
        private SpriteFont _choice2;
        private SpriteFont _choice3;
        private SpriteFont _choice4;
        private SpriteFont _choice5;
        private SpriteFont _choice6;
        private SpriteFont _choice7;
        private SpriteFont _choice8;



        private Random random = new Random();

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouse;

        private bool _isHovering;

        private double _successTimer;

        private Button[] _buttons1;

        private Button[] _buttons2;

        private int _selectedButtonIndex = 0;

        private string _button1 = "";
        private string _button2 = "";
        private string _button3 = "";
        private string _button4 = "";
        private string _button5 = "";
        private string _button6 = "";
        private string _button7 = "";
        private string _button8 = "";

        private float _bombAnimationTimer;
        private short _bombFrame;

        public string MiniGameProblem;

        public string Half1 = "";

        public string Half2 = "";

        public string Answer = "";

        public bool Succeeded;

        public bool LeftSelect;

        public bool RightSelect;

        public int ButtonsPressed = 0;

        public int RandomNum;

        public float InvalidAnswer = 0;

        public LockedDoorPy Door;

        public Minigame4(LockedDoorPy LockedDoor)
        {
            Door = LockedDoor;
        }
        public override void Activate()
        {
            _graphics = ScreenManager.Game.GraphicsDevice;
            _spriteBatch = ScreenManager.SpriteBatch;
            Succeeded = false;
            LeftSelect = false;
            RightSelect = false;
            _successTimer = 0;
            _bombAnimationTimer = 0;


            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _screen = _content.Load<Texture2D>("MinigameDoorScreen");
            _success = _content.Load<Texture2D>("Sprite_success");
            _invalid = _content.Load<Texture2D>("Sprite_invalid");
            _objective = _content.Load<SpriteFont>("MG2objective");
            _bomb = _content.Load<Texture2D>("Sprite_bomb");
            _problem = _content.Load<SpriteFont>("MG2problem");
            _answer1 = _content.Load<SpriteFont>("MG2answer");
            _answer2 = _content.Load<SpriteFont>("MG2answer");

            _choice1 = _content.Load<SpriteFont>("MG2choice");
            _choice2 = _content.Load<SpriteFont>("MG2choice");
            _choice3 = _content.Load<SpriteFont>("MG2choice");
            _choice4 = _content.Load<SpriteFont>("MG2choice");
            _choice5 = _content.Load<SpriteFont>("MG2choice");
            _choice6 = _content.Load<SpriteFont>("MG2choice");
            _choice7 = _content.Load<SpriteFont>("MG2choice");
            _choice8 = _content.Load<SpriteFont>("MG2choice");

            _buttons1 = new Button[]
             {
                 new Button(new Vector2(100, 400)),
                 new Button(new Vector2(0, 500)),
                 new Button(new Vector2(200, 500)),
                 new Button(new Vector2(100, 600))
             };
            foreach (var b in _buttons1) b.LoadContent(_content);

            _buttons2 = new Button[]
             {
                 new Button(new Vector2(975, 400)),
                 new Button(new Vector2(875, 500)),
                 new Button(new Vector2(1075, 500)),
                 new Button(new Vector2(975, 600))
             };
            foreach (var b in _buttons2) b.LoadContent(_content);

            //_debugCircle = _content.Load<Texture2D>("circle");

            RandomNum = random.Next(1, 3);
            if (RandomNum == 1)
            {
                MiniGameProblem = "What are the sorting algorithms mentioned \n " +
                    "in the textbook?";
                _button1 = _button1 + "Insert ";
                _button2 = _button2 + "Select ";
                _button3 = _button3 + "Quick ";
                _button4 = _button4 + "Heap ";
                _button5 = _button5 + "Bubble ";
                _button6 = _button6 + "Bucket ";
                _button7 = _button7 + "Shell ";
                _button8 = _button8 + "Merge ";
            }
            else
            {
                MiniGameProblem = "What are the operations commonly used in Python? \n" +
                    "Besides '+'?";
                _button1 = _button1 + "^ ";
                _button2 = _button2 + "( ";
                _button3 = _button3 + "/ ";
                _button4 = _button4 + "- ";
                _button5 = _button5 + ") ";
                _button6 = _button6 + "* ";
                _button7 = _button7 + "% ";
                _button8 = _button8 + "! ";
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

            if (!Succeeded)
            {
                _previousKeyboardState = _currentKeyboardState;
                _currentKeyboardState = Keyboard.GetState();
                Keys[] keys = _currentKeyboardState.GetPressedKeys();
                if (keys.Length > 0 && !_previousKeyboardState.IsKeyDown(keys[0]))
                {
                    switch (keys[0])
                    {
                        case Keys.W:
                            ButtonsPressed++;
                            _buttons1[0].IsPressed = true;
                            Answer = Answer + _button1;
                            break;
                        case Keys.A:
                            ButtonsPressed++;
                            _buttons1[1].IsPressed = true;
                            Answer = Answer + _button2;
                            break;
                        case Keys.D:
                            ButtonsPressed++;
                            _buttons1[2].IsPressed = true;
                            Answer = Answer + _button3;
                            break;
                        case Keys.S:
                            ButtonsPressed++;
                            _buttons1[3].IsPressed = true;
                            Answer = Answer + _button4;
                            break;
                        default:
                            break;
                    }
                }
                if (keys.Length > 0 && !_previousKeyboardState.IsKeyDown(keys[0]))
                {
                    switch (keys[0])
                    {
                        case Keys.Up:
                            ButtonsPressed++;
                            _buttons2[0].IsPressed = true;
                            Answer = Answer + _button5;
                            break;
                        case Keys.Left:
                            ButtonsPressed++;
                            _buttons2[1].IsPressed = true;
                            Answer = Answer + _button6;
                            break;
                        case Keys.Right:
                            ButtonsPressed++;
                            _buttons2[2].IsPressed = true;
                            Answer = Answer + _button7;
                            break;
                        case Keys.Down:
                            ButtonsPressed++;
                            _buttons2[3].IsPressed = true;
                            Answer = Answer + _button8;
                            break;
                        default:
                            break;
                    }
                }
                if (ButtonsPressed == 4)
                {
                    if (RandomNum == 1 && Answer.Contains("Insert") && Answer.Contains("Quick") && Answer.Contains("Bubble") && Answer.Contains("Merge")) Succeeded = true;
                    else if (RandomNum == 2 && Answer.Contains("-") && Answer.Contains("*") && Answer.Contains("/") && Answer.Contains("%")) Succeeded = true;
                    else
                    {
                        
                        foreach(var b in _buttons1)
                        {
                            if (b.IsPressed) b.IsPressed = false;
                        }
                        foreach (var b in _buttons2)
                        {
                            if (b.IsPressed) b.IsPressed = false;
                        }
                        InvalidAnswer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (InvalidAnswer > 2)
                        {
                            Answer = "";
                            
                            InvalidAnswer = 0;
                            ButtonsPressed = 0;
                        }

                    }
                }
            }

            if (Succeeded)
            {
                _successTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_successTimer > 4)
                {
                    Door.State = doorState.Opening;
                    ExitScreen();
                    ScreenManager.RemoveScreen(this);
                }

            }
        }

        public override void Draw(GameTime gameTime)
        {

            _bombAnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var bombSource = new Rectangle(_bombFrame * 193, 0, 192, 264);
            if(_bombAnimationTimer > 0.1 && _bombFrame < 4)
            {
                _bombFrame++;
                if(_bombFrame > 3)
                {
                    _bombFrame = 0;
                }
                _bombAnimationTimer = 0;
            }
            if (Succeeded) _bombFrame = 4;

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_screen, new Rectangle(0, 0, 800, 500), Color.White);
            _spriteBatch.DrawString(_objective, "DEFUSE THE BOMB", new Vector2(150, 25), Color.Red);
            _spriteBatch.DrawString(_objective, "Select up to 4 buttons.", new Vector2(70, 80), Color.Black, 0f, new Vector2(0, 0), 0.45f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_objective, "W A S D \nfor the left side...", new Vector2(25, 200), Color.Black, 0f, new Vector2(0, 0), 0.40f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_objective, "Arrow keys \nfor the right!", new Vector2(475, 200), Color.Black, 0f, new Vector2(0, 0), 0.40f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_problem, MiniGameProblem, new Vector2(60, 125), Color.Black, 0f, new Vector2(0, 0), 0.70f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_bomb, new Vector2(280, 200), bombSource, Color.White);
            foreach (var b in _buttons1)
            {
                b.Draw(gameTime, _spriteBatch);
                //_spriteBatch.Draw(_debugCircle, new Vector2(40, 150), Color.Red);
            }
            foreach (var b in _buttons2)
            {
                b.Draw(gameTime, _spriteBatch);
            }
            if (RandomNum == 1)
            {
                _spriteBatch.DrawString(_choice1, _button1, new Vector2(69, 270), Color.Black, 0f, new Vector2(0, 0), 0.80f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_choice2, _button2, new Vector2(3, 335), Color.Black, 0f, new Vector2(0, 0), 0.80f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_choice3, _button3, new Vector2(137, 335), Color.Black, 0f, new Vector2(0, 0), 0.80f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_choice4, _button4, new Vector2(69, 400), Color.Black, 0f, new Vector2(0, 0), 0.80f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_choice5, _button5, new Vector2(639, 270), Color.Black, 0f, new Vector2(0, 0), 0.80f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_choice6, _button6, new Vector2(573, 335), Color.Black, 0f, new Vector2(0, 0), 0.80f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_choice7, _button7, new Vector2(703, 335), Color.Black, 0f, new Vector2(0, 0), 0.80f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(_choice8, _button8, new Vector2(639, 400), Color.Black, 0f, new Vector2(0, 0), 0.80f, SpriteEffects.None, 0);
            }
            else
            {
                _spriteBatch.DrawString(_choice1, _button1, new Vector2(69, 270), Color.Black);
                _spriteBatch.DrawString(_choice2, _button2, new Vector2(3, 335), Color.Black);
                _spriteBatch.DrawString(_choice3, _button3, new Vector2(137, 335), Color.Black);
                _spriteBatch.DrawString(_choice4, _button4, new Vector2(69, 400), Color.Black);
                _spriteBatch.DrawString(_choice5, _button5, new Vector2(639, 270), Color.Black);
                _spriteBatch.DrawString(_choice6, _button6, new Vector2(573, 335), Color.Black);
                _spriteBatch.DrawString(_choice7, _button7, new Vector2(703, 335), Color.Black);
                _spriteBatch.DrawString(_choice8, _button8, new Vector2(639, 400), Color.Black);
            }

            if(RandomNum ==1)_spriteBatch.DrawString(_answer1, Answer, new Vector2(240, 360), Color.White, 0f, new Vector2(0,0), 0.45f, SpriteEffects.None, 0);
            else _spriteBatch.DrawString(_answer1, Answer, new Vector2(280, 360), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);


            if (InvalidAnswer > 0) _spriteBatch.Draw(_invalid, new Rectangle(300, 400, 200, 110), Color.White);
            if (Succeeded) _spriteBatch.Draw(_success, new Rectangle(300, 120, 200, 100), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
