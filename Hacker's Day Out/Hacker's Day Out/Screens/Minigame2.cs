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
        private Texture2D _invalid;
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
        private MouseState  _currentMouseState;
        private MouseState _previousMouse;

        private bool _isHovering;

        private double _successTimer;

        private Button[] _buttons1;

        private Button[] _buttons2;

        private int _selectedButtonIndex = 0;

        private int _book;

        private string _button1 = "";
        private string _button2 = "";
        private string _button3 = "";
        private string _button4 = "";
        private string _button5 = "";
        private string _button6 = "";
        private string _button7 = "";
        private string _button8 = "";

        public string MiniGameProblem;

        public string Half1 = "";

        public string Half2 = "";

        public string Answer = "";

        public bool Succeeded;

        public bool LeftSelect;

        public bool RightSelect;

        public int RandomNum;

        public float InvalidAnswer = 0;

        public LockedDoorPy Door;

        public Minigame2(LockedDoorPy LockedDoor, int Book)
        {
            Door = LockedDoor;
            _book = Book;
         }
        public override void Activate()
        {
            _graphics = ScreenManager.Game.GraphicsDevice;
            _spriteBatch = ScreenManager.SpriteBatch;
            Succeeded = false;
            LeftSelect = false;
            RightSelect = false;
            _successTimer = 0;

            
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _screen = _content.Load<Texture2D>("MinigameDoorScreen");
            _success = _content.Load<Texture2D>("Sprite_success");
            _invalid = _content.Load<Texture2D>("Sprite_invalid");
            _objective = _content.Load<SpriteFont>("MG2objective");
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

            switch (_book)
            {
                case (1):
                    if (RandomNum == 1)
                    {
                        MiniGameProblem = "The hypothetical computing \n" +
                            "machine was made in 1936 by\n" +
                            "Alan _________ ";
                        _button1 = _button1 + "Jack";
                        _button2 = _button2 + "Tur";
                        _button3 = _button3 + "Harr";
                        _button4 = _button4 + "Jon";
                        _button5 = _button5 + "ing";
                        _button6 = _button6 + "son";
                        _button7 = _button7 + "es";
                        _button8 = _button8 + "ison";
                    }
                    else
                    {
                        MiniGameProblem = "The fastest algorithm to sort \n " + "in Python is \n" +
                            "_________ sort";
                        _button1 = _button1 + "Inser";
                        _button2 = _button2 + "Qui";
                        _button3 = _button3 + "Bub";
                        _button4 = _button4 + "Mer";
                        _button5 = _button5 + "ge";
                        _button6 = _button6 + "ck";
                        _button7 = _button7 + "tion";
                        _button8 = _button8 + "ble";
                    }
                    break;
                case (2):
                    if (RandomNum == 1)
                    {
                        MiniGameProblem = "Java was originally \n" +
                            "developed in May 1995 by\n" +
                            "James _________ ";
                        _button1 = _button1 + "Gos";
                        _button2 = _button2 + "Tur";
                        _button3 = _button3 + "Robi";
                        _button4 = _button4 + "Con";
                        _button5 = _button5 + "ing";
                        _button6 = _button6 + "ns";
                        _button7 = _button7 + "roy";
                        _button8 = _button8 + "ling";
                    }
                    else
                    {
                        MiniGameProblem = "In object oriented programming, \n " + "an instance of a class is \n" +
                            "an _________";
                        _button1 = _button1 + "vari";
                        _button2 = _button2 + "cla";
                        _button3 = _button3 + "str";
                        _button4 = _button4 + "Obj";
                        _button5 = _button5 + "ect";
                        _button6 = _button6 + "ss";
                        _button7 = _button7 + "ing";
                        _button8 = _button8 + "ble";
                    }
                    break;
                case (3):
                    if (RandomNum == 1)
                    {
                        MiniGameProblem = "C is a general-purpose\n" +
                            "language made in 1972 by\n" +
                            "Dennis _________ ";
                        _button1 = _button1 + "Be";
                        _button2 = _button2 + "Tur";
                        _button3 = _button3 + "Gos";
                        _button4 = _button4 + "Ritc";
                        _button5 = _button5 + "an";
                        _button6 = _button6 + "ing";
                        _button7 = _button7 + "hie";
                        _button8 = _button8 + "ling";
                    }
                    else
                    {
                        MiniGameProblem = "In C, \n " + "the & symbol gets a variable's \n" +
                            "memory _________";
                        _button1 = _button1 + "add";
                        _button2 = _button2 + "ca";
                        _button3 = _button3 + "di";
                        _button4 = _button4 + "but";
                        _button5 = _button5 + "sk";
                        _button6 = _button6 + "ress";
                        _button7 = _button7 + "ton";
                        _button8 = _button8 + "rd";
                    }
                    break;
                case (4):
                    if (RandomNum == 1)
                    {
                        MiniGameProblem = "C# was developed\n" +
                            "in 2002 by\n" +
                            "_________ ";
                        _button1 = _button1 + "Ritc";
                        _button2 = _button2 + "Kon";
                        _button3 = _button3 + "Micro";
                        _button4 = _button4 + "App";
                        _button5 = _button5 + "soft";
                        _button6 = _button6 + "le";
                        _button7 = _button7 + "ami";
                        _button8 = _button8 + "hie";
                    }
                    else
                    {
                        MiniGameProblem = "In C#, \n " + "what does it mean for a class to \n extend another? \n" +
                            "_________";
                        _button1 = _button1 + "Inh";
                        _button2 = _button2 + "Poi";
                        _button3 = _button3 + "Over";
                        _button4 = _button4 + "Abst";
                        _button5 = _button5 + "ride";
                        _button6 = _button6 + "ract";
                        _button7 = _button7 + "erit";
                        _button8 = _button8 + "nt";
                    }
                    break;
                default:
                    break;
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
                if (!LeftSelect && keys.Length > 0 && !_previousKeyboardState.IsKeyDown(keys[0]))
                {
                    switch (keys[0])
                    {
                        case Keys.W:
                            LeftSelect = true;
                            _buttons1[0].IsPressed = true;
                            Half1 = Half1 + _button1;
                            break;
                        case Keys.A:
                            LeftSelect = true;
                            _buttons1[1].IsPressed = true;
                            Half1 = Half1 + _button2;
                            break;
                        case Keys.D:
                            LeftSelect = true;
                            _buttons1[2].IsPressed = true;
                            Half1 = Half1 + _button3;
                            break;
                        case Keys.S:
                            LeftSelect = true;
                            _buttons1[3].IsPressed = true;
                            Half1 = Half1 + _button4;
                            break;
                        default:
                            break;
                    }
                }
                if (!RightSelect && keys.Length > 0 && !_previousKeyboardState.IsKeyDown(keys[0]))
                {
                    switch (keys[0])
                    {
                        case Keys.Up:
                            RightSelect = true;
                            _buttons2[0].IsPressed = true;
                            Half2 = Half2 + _button5;
                            break;
                        case Keys.Left:
                            RightSelect = true;
                            _buttons2[1].IsPressed = true;
                            Half2 = Half2 + _button6;
                            break;
                        case Keys.Right:
                            RightSelect = true;
                            _buttons2[2].IsPressed = true;
                            Half2 = Half2 + _button7;
                            break;
                        case Keys.Down:
                            RightSelect = true;
                            _buttons2[3].IsPressed = true;
                            Half2 = Half2 + _button8;
                            break;
                        default:
                            break;
                    }
                }
                if (_currentKeyboardState.IsKeyDown(Keys.Escape))
                {
                    ExitScreen();
                    ScreenManager.RemoveScreen(this);
                }
                if (LeftSelect && RightSelect)
                {
                    Answer = Answer + Half1 + Half2;
                    switch(_book)
                    {
                        case (1):
                            if (RandomNum == 1 && Answer == "Turing") Succeeded = true;
                            else if (RandomNum == 2 && Answer == "Quick") Succeeded = true;
                            else
                            {
                                InvalidAnswer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                                if (InvalidAnswer > 2)
                                {
                                    Answer = "";
                                    Half1 = "";
                                    Half2 = "";
                                    LeftSelect = false;
                                    RightSelect = false;
                                    InvalidAnswer = 0;
                                }
                            }
                            break;
                        case (2):
                            if (RandomNum == 1 && Answer == "Gosling") Succeeded = true;
                            else if (RandomNum == 2 && Answer == "Object") Succeeded = true;
                            else
                            {
                                InvalidAnswer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                                if (InvalidAnswer > 2)
                                {
                                    Answer = "";
                                    Half1 = "";
                                    Half2 = "";
                                    LeftSelect = false;
                                    RightSelect = false;
                                    InvalidAnswer = 0;
                                }
                            }
                            break;
                        case (3):
                            if (RandomNum == 1 && Answer == "Ritchie") Succeeded = true;
                            else if (RandomNum == 2 && Answer == "address") Succeeded = true;
                            else
                            {
                                InvalidAnswer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                                if (InvalidAnswer > 2)
                                {
                                    Answer = "";
                                    Half1 = "";
                                    Half2 = "";
                                    LeftSelect = false;
                                    RightSelect = false;
                                    InvalidAnswer = 0;
                                }
                            }
                            break;
                        case (4):
                            if (RandomNum == 1 && Answer == "Microsoft") Succeeded = true;
                            else if (RandomNum == 2 && Answer == "Inherit") Succeeded = true;
                            else
                            {
                                InvalidAnswer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                                if (InvalidAnswer > 2)
                                {
                                    Answer = "";
                                    Half1 = "";
                                    Half2 = "";
                                    LeftSelect = false;
                                    RightSelect = false;
                                    InvalidAnswer = 0;
                                }
                            }
                            break;
                        default:
                            break;
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
           

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_screen, new Rectangle(0, 0, 800, 500), Color.White);
            _spriteBatch.DrawString(_objective, "Find the correct word", new Vector2(70, 75), Color.Red);
            _spriteBatch.DrawString(_objective, "Use W A S D \nfor the left side...", new Vector2(45, 200), Color.Black, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_objective, "And the arrow keys \nfor the right!", new Vector2(500, 200), Color.Black, 0f, new Vector2(0, 0), 0.45f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_problem, MiniGameProblem, new Vector2(100, 125), Color.Black);
            foreach(var b in _buttons1)
            {
                b.Draw(gameTime, _spriteBatch);
                //_spriteBatch.Draw(_debugCircle, new Vector2(40, 150), Color.Red);
            }
            foreach(var b in _buttons2)
            {
                b.Draw(gameTime, _spriteBatch);
            }
            _spriteBatch.DrawString(_choice1, _button1, new Vector2(69, 270), Color.Black);
            _spriteBatch.DrawString(_choice2, _button2, new Vector2(3, 335), Color.Black);
            _spriteBatch.DrawString(_choice3, _button3, new Vector2(137, 335), Color.Black);
            _spriteBatch.DrawString(_choice4, _button4, new Vector2(69, 400), Color.Black);
            _spriteBatch.DrawString(_choice5, _button5, new Vector2(639, 270), Color.Black);
            _spriteBatch.DrawString(_choice6, _button6, new Vector2(573, 335), Color.Black);
            _spriteBatch.DrawString(_choice7, _button7, new Vector2(703, 335), Color.Black);
            _spriteBatch.DrawString(_choice8, _button8, new Vector2(639, 400), Color.Black);

            _spriteBatch.DrawString(_answer1, Half1, new Vector2(270, 360), Color.Black);
            _spriteBatch.DrawString(_answer2, Half2, new Vector2(430, 360), Color.Black);

            if (InvalidAnswer > 0) _spriteBatch.Draw(_invalid, new Rectangle(300, 400, 200, 110), Color.White);
            if (Succeeded) _spriteBatch.Draw(_success, new Rectangle(300, 200, 200, 150), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
