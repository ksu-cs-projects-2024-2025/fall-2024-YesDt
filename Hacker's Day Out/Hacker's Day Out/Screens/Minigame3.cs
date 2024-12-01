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
using Microsoft.Xna.Framework.Media;

namespace HackersDayOut.Screens
{

    public class Minigame3 : GameScreen
    {
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        private Texture2D _screen;
        private Texture2D _cannon;
        private Texture2D _firewall;
        private Texture2D _success;

        private SpriteFont _objective;
        private SpriteFont _problem;
        private SpriteFont _answer;


        private Random random = new Random();

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private double _successTimer;

        private double _cannonTimer;

        private double _firewallTimer;

        private short _cannonFrame = 0;

        private short _firewallFrame = 0;

        private bool _cannonGoOff;

        private int _book;

        public string MiniGameProblem;

        public string Answer = " ";

        public bool succeeded;

        public int RandomNum;
        public Minigame3(int Book)
        {
            _book = Book;
        }

        public override void Activate()
        {
            _graphics = ScreenManager.Game.GraphicsDevice;
            _spriteBatch = ScreenManager.SpriteBatch;

            _cannonGoOff = false;
            succeeded = false;
            _successTimer = 0;


            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _screen = _content.Load<Texture2D>("MinigameCompScreen");
            _cannon = _content.Load<Texture2D>("Sprite_cannon");
            _firewall = _content.Load<Texture2D>("Sprite_firewall");
            _success = _content.Load<Texture2D>("Sprite_success");
            _objective = _content.Load<SpriteFont>("MG1objective");
            _problem = _content.Load<SpriteFont>("MG1problem");
            _answer = _content.Load<SpriteFont>("MG1answer");


            RandomNum = random.Next(1, 3);
            switch(_book)
            {
                case 1:
                    if (RandomNum == 1)
                    {
                        MiniGameProblem = "list = [3,6,9] \n" +
                            "sum = 0 \n" +
                            "for i in list: \n" +
                            "   sum = sum + i \n" +
                            "What is sum after the loop?";
                    }
                    else
                    {
                        MiniGameProblem = "number1 = 2\n" +
                            "number2 = 5 \n" +
                            "number3 = number1 * number2\n" +
                            "number3 = number2\n" +
                            "What is number3 now?";

                    }
                    break;
                case (2):
                    if (RandomNum == 1)
                    {
                        MiniGameProblem = "int[] list = new int[5] \n" +
                           
                            "What is the first index of elements?";
                    }
                    else
                    {
                        MiniGameProblem = "char[] word = new char[11]\n" +
                            "word = {'intelligent'}\n" +
                            "What is char[4] (the char at index 4)?";

                    }
                    break;
                case (3):
                    if (RandomNum == 1)
                    {
                        MiniGameProblem = "int num = 10 \n" +
                            "int i = 0\n" +
                            "while(i < num)\n" +
                            "Which variable do you update \nto avoid an infinite loop?";
                    }
                    else
                    {
                        MiniGameProblem = "if (num == 1)\n" +
                            "else if (num == 2)\n" +
                            "else if (num == 3)\n" +
                            "what is a simplified type of loop?";

                    }
                    break;
                case (4):
                    if (RandomNum == 1)
                    {
                        MiniGameProblem = "public class Question3 \n" +

                            "Which modifier should the class be \nso only derived classes can access?";
                    }
                    else
                    {
                        MiniGameProblem = "public abstract class Question4\n" +
                            "   virtual int number \n" +
                            "public class InheritingClass: Question4\n" +
                            "what modifier should this class use \n to override Question4's virtual int?";

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

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // TODO: Add your update logic here
            if (!succeeded)
            {
                _previousKeyboardState = _currentKeyboardState;
                _currentKeyboardState = Keyboard.GetState();
                #region KeyboardInputs

                bool shift = _currentKeyboardState.IsKeyDown(Keys.LeftShift) || _currentKeyboardState.IsKeyDown(Keys.RightShift);
                if (_currentKeyboardState.IsKeyDown(Keys.A) && _previousKeyboardState.IsKeyUp(Keys.A)) Answer = Answer + 'a';
                if (_currentKeyboardState.IsKeyDown(Keys.B) && _previousKeyboardState.IsKeyUp(Keys.B)) Answer = Answer + 'b';
                if (_currentKeyboardState.IsKeyDown(Keys.C) && _previousKeyboardState.IsKeyUp(Keys.C)) Answer = Answer + 'c';
                if (_currentKeyboardState.IsKeyDown(Keys.D) && _previousKeyboardState.IsKeyUp(Keys.D)) Answer = Answer + 'd';
                if (_currentKeyboardState.IsKeyDown(Keys.E) && _previousKeyboardState.IsKeyUp(Keys.E)) Answer = Answer + 'e';
                if (_currentKeyboardState.IsKeyDown(Keys.F) && _previousKeyboardState.IsKeyUp(Keys.F)) Answer = Answer + 'f';
                if (_currentKeyboardState.IsKeyDown(Keys.G) && _previousKeyboardState.IsKeyUp(Keys.G)) Answer = Answer + 'g';
                if (_currentKeyboardState.IsKeyDown(Keys.H) && _previousKeyboardState.IsKeyUp(Keys.H)) Answer = Answer + 'h';
                if (_currentKeyboardState.IsKeyDown(Keys.I) && _previousKeyboardState.IsKeyUp(Keys.I)) Answer = Answer + 'i';
                if (_currentKeyboardState.IsKeyDown(Keys.J) && _previousKeyboardState.IsKeyUp(Keys.J)) Answer = Answer + 'j';
                if (_currentKeyboardState.IsKeyDown(Keys.K) && _previousKeyboardState.IsKeyUp(Keys.K)) Answer = Answer + 'k';
                if (_currentKeyboardState.IsKeyDown(Keys.L) && _previousKeyboardState.IsKeyUp(Keys.L)) Answer = Answer + 'l';
                if (_currentKeyboardState.IsKeyDown(Keys.M) && _previousKeyboardState.IsKeyUp(Keys.M)) Answer = Answer + 'm';
                if (_currentKeyboardState.IsKeyDown(Keys.N) && _previousKeyboardState.IsKeyUp(Keys.N)) Answer = Answer + 'n';
                if (_currentKeyboardState.IsKeyDown(Keys.O) && _previousKeyboardState.IsKeyUp(Keys.O)) Answer = Answer + 'o';
                if (_currentKeyboardState.IsKeyDown(Keys.P) && _previousKeyboardState.IsKeyUp(Keys.P)) Answer = Answer + 'p';
                if (_currentKeyboardState.IsKeyDown(Keys.Q) && _previousKeyboardState.IsKeyUp(Keys.Q)) Answer = Answer + 'q';
                if (_currentKeyboardState.IsKeyDown(Keys.R) && _previousKeyboardState.IsKeyUp(Keys.R)) Answer = Answer + 'r';
                if (_currentKeyboardState.IsKeyDown(Keys.S) && _previousKeyboardState.IsKeyUp(Keys.S)) Answer = Answer + 's';
                if (_currentKeyboardState.IsKeyDown(Keys.T) && _previousKeyboardState.IsKeyUp(Keys.T)) Answer = Answer + 't';
                if (_currentKeyboardState.IsKeyDown(Keys.U) && _previousKeyboardState.IsKeyUp(Keys.U)) Answer = Answer + 'u';
                if (_currentKeyboardState.IsKeyDown(Keys.V) && _previousKeyboardState.IsKeyUp(Keys.V)) Answer = Answer + 'v';
                if (_currentKeyboardState.IsKeyDown(Keys.W) && _previousKeyboardState.IsKeyUp(Keys.W)) Answer = Answer + 'w';
                if (_currentKeyboardState.IsKeyDown(Keys.X) && _previousKeyboardState.IsKeyUp(Keys.X)) Answer = Answer + 'x';
                if (_currentKeyboardState.IsKeyDown(Keys.Y) && _previousKeyboardState.IsKeyUp(Keys.Y)) Answer = Answer + 'y';
                if (_currentKeyboardState.IsKeyDown(Keys.Z) && _previousKeyboardState.IsKeyUp(Keys.Z)) Answer = Answer + 'z';
                if (_currentKeyboardState.IsKeyDown(Keys.D1) && _previousKeyboardState.IsKeyUp(Keys.D1))
                {
                    if (shift) Answer = Answer + Answer + '!';
                    else Answer = Answer + '1';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D2) && _previousKeyboardState.IsKeyUp(Keys.D2))
                {
                    if (shift) Answer = Answer + '@';
                    else Answer = Answer + '2';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D3) && _previousKeyboardState.IsKeyUp(Keys.D3))
                {
                    if (shift) Answer = Answer + '#';
                    else Answer = Answer + '3';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D4) && _previousKeyboardState.IsKeyUp(Keys.D4))
                {
                    if (shift) Answer = Answer + '$';
                    else Answer = Answer + '4';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D5) && _previousKeyboardState.IsKeyUp(Keys.D5))
                {
                    if (shift) Answer = Answer + '%';
                    else Answer = Answer + '5';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D6) && _previousKeyboardState.IsKeyUp(Keys.D6))
                {
                    if (shift) Answer = Answer + '^';
                    else Answer = Answer + '6';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D7) && _previousKeyboardState.IsKeyUp(Keys.D7))
                {
                    if (shift) Answer = Answer + '&';
                    else Answer = Answer + '7';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D8) && _previousKeyboardState.IsKeyUp(Keys.D8))
                {
                    if (shift) Answer = Answer + '*';
                    else Answer = Answer + '8';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D9) && _previousKeyboardState.IsKeyUp(Keys.D9))
                {
                    if (shift) Answer = Answer + '(';
                    else Answer = Answer + '9';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D0) && _previousKeyboardState.IsKeyUp(Keys.D0))
                {
                    if (shift) Answer = Answer + ')';
                    else Answer = Answer + '0';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space)) Answer = Answer + ' ';
                if (_currentKeyboardState.IsKeyDown(Keys.OemMinus) && _previousKeyboardState.IsKeyUp(Keys.OemMinus))
                {
                    if (shift) Answer = Answer + '_';
                    else Answer = Answer + '-';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.OemPlus) && _previousKeyboardState.IsKeyUp(Keys.OemPlus))
                {
                    if (shift) Answer = Answer + '+';
                    else Answer = Answer + '=';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.OemComma) && _previousKeyboardState.IsKeyUp(Keys.OemComma))
                {
                    if (shift) Answer = Answer + '<';
                    else Answer = Answer + ',';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.OemPeriod) && _previousKeyboardState.IsKeyUp(Keys.OemPeriod))
                {
                    if (shift) Answer = Answer + '>';
                    else Answer = Answer + '.';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.OemQuestion) && _previousKeyboardState.IsKeyUp(Keys.OemQuestion))
                {
                    if (shift) Answer = Answer + '?';
                    else Answer = Answer + '/';
                }
                if (_currentKeyboardState.IsKeyDown(Keys.Back) && _previousKeyboardState.IsKeyUp(Keys.Back))
                {
                    if (Answer.Length > 0) Answer = Answer.Remove(Answer.Length - 1);
                }
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                ExitScreen();
                ScreenManager.RemoveScreen(this);
            }

            #endregion
            switch(_book)
            {
                case (1):
                    if ((RandomNum == 1) && ((Answer == "18") || Answer == " 18"))
                    {
                        _cannonGoOff = true;                  
                    }
                    if ((RandomNum == 2) && ((Answer == "5") || Answer == " 5"))
                    {
                        _cannonGoOff = true;
                    }
                    break;
                case (2):
                    if ((RandomNum == 1) && ((Answer == "0")))
                    {
                        _cannonGoOff = true;
                    }
                    if ((RandomNum == 2) && ((Answer == "l")))
                    {
                        _cannonGoOff = true;
                    }
                    break;
                case (3):
                    if ((RandomNum == 1) && ((Answer == "i")))
                    {
                        _cannonGoOff = true;
                    }
                    if ((RandomNum == 2) && ((Answer == "switch" || Answer == "Switch")))
                    {
                        _cannonGoOff = true;
                    }
                    break;
                case (4):
                    if ((RandomNum == 1) && ((Answer == "protected") || (Answer == "Protected")))
                    {
                        _cannonGoOff = true;
                    }
                    if ((RandomNum == 2) && ((Answer == "override" || Answer == "Override")))
                    {
                        _cannonGoOff = true;
                    }
                    break;
                default:
                    break;
            }
            
            

            if (succeeded)
            {
                _successTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_successTimer > 4)
                {
                    switch (_book)
                    {
                        case (1):
                            ScreenManager.PythonCodeCollected = true;
                            break;
                        case (2):
                            ScreenManager.JavaCodeCollected = true;
                            break;
                        case (3):
                            ScreenManager.CCodeCollected = true;
                            break;
                        case (4):
                            ScreenManager.CSharpCodeCollected = true;
                            break;
                        default:
                            break;
                    }
                    ExitScreen();
                    ScreenManager.RemoveScreen(this);
                }

            }
        }


        public override void Draw(GameTime gameTime)
        {
            var cSource = new Rectangle(_cannonFrame * 296, 0, 296, 157);
            var fSource = new Rectangle(_firewallFrame * 257, 0, 256, 304);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_screen, new Rectangle(0, 0, 800, 500), Color.White);
            _spriteBatch.DrawString(_objective, "DESTROY THE FIRE WALL", new Vector2(320, 280), Color.Red, 0f, new Vector2(200, 200), 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(_objective, "Answer the question!", new Vector2(350, 300), Color.Red, 0f, new Vector2(200, 200), 0.75f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(_problem, MiniGameProblem, new Vector2(200, 250), Color.Black);
            
            if(_cannonGoOff)
            {
                _cannonTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(_cannonTimer > 0.01 && _cannonFrame < 5)
                {
                    _cannonFrame++;
                    if (_cannonFrame == 3) _firewallFrame = 3;
                    _cannonTimer = 0;
                }
                if(_cannonFrame == 5)
                {
                    succeeded = true;
                }
            }
            _firewallTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_firewallTimer > 0.1 && _firewallFrame < 3)
            {
                _firewallFrame++;
                if (_firewallFrame > 2 && !_cannonGoOff)
                {
                    _firewallFrame = 0;

                }
                _firewallTimer = 0;
            }
            else if(_firewallTimer > 0.1 && _firewallFrame == 3)
            {
                _firewallFrame++;
            }
            
            _spriteBatch.Draw(_cannon, new Vector2(40, 330), cSource, Color.White);
            _spriteBatch.Draw(_firewall, new Vector2(580, 190), fSource, Color.White);
            _spriteBatch.DrawString(_answer, Answer, new Vector2(280, 350), Color.White);

            if (succeeded) _spriteBatch.Draw(_success, new Rectangle(500, 100, 300, 300), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
