using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using HackersDayOut.StateManagement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using HackersDayOut.Collisions;

namespace HackersDayOut.Screens
{
    public class ComputerRoom : GameScreen
    {
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        private Texture2D _compLab;
        private Texture2D _overlay;

        private Student _student = new Student(new Vector2(200, 280));

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private BoundingRectangle[] _boundaries;

        public Texture2D circle;

        public BoundingCircle cir;

        public BoundingCircle cir2;

        public Computer[] computers;

        public LockedDoorPy Door;

        public bool ComputerCode = false; 

        public static PythonBook pyBook;

        public static bool PythonCodeCollected = false;


        public ComputerRoom()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        public override void Activate()
        {
            _graphics = ScreenManager.Game.GraphicsDevice;
            _spriteBatch = ScreenManager.SpriteBatch;
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _student.LoadContent(_content);
            _compLab = _content.Load<Texture2D>("Sprite-comlabv1");
            _overlay = _content.Load<Texture2D>("Sprite-comlabv1overlay");
            circle = _content.Load<Texture2D>("circle");
           

            pyBook = new PythonBook(new Vector2(400, 300));
            pyBook.LoadContent(_content);

            _boundaries = new BoundingRectangle[]
                {
                    new BoundingRectangle(50, -100, 40, 1000),
                    new BoundingRectangle(0, 240, 1090, 14),
                    new BoundingRectangle(1040, 224, 40, 40),
                    new BoundingRectangle(1040, 400, 40, 400),
                    new BoundingRectangle(0, 500, 1090, 70),
                    new BoundingRectangle(140, 230, 220, 70),
                    new BoundingRectangle(340, 230, 220, 70),
                    new BoundingRectangle(540, 230, 220, 70),
                    new BoundingRectangle(740, 230, 220, 70),


                };

            cir = new BoundingCircle(new Vector2(240, 200), 10f);
            cir2 = new BoundingCircle(new Vector2(1050, 280), 50f);
            computers = new Computer[]
            {
                new Computer(new Vector2(240, 140)),
            };
            foreach (var c in computers)
            {
                c.LoadContent(_content);
            }
            Door = new LockedDoorPy(new Vector2(1015, 250), new BoundingRectangle(1050, 260, 140, 130), false);
            Door.LoadContent(_content);
            PythonCodeCollected = false;

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

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {

            }

        }
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                MediaPlayer.Pause();
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                MediaPlayer.Resume();

                _student.Update(gameTime);
                foreach (var r in _boundaries)
                {
                    _student.CollisionHandling(r);
                }
                _student.CollisionHandling(Door.Bounds);
                if(pyBook.Collected && !PythonCodeCollected)
                {
                    _student.InteractHandlingOne(cir);
                }
                
                if (Door.State == doorState.Closed && PythonCodeCollected)
                {
                    _student.InteractHandlingTwo(cir2);
                }
                if(Door.State != doorState.Closed)
                {
                    Door.Bounds = new BoundingRectangle(-10000, -100000, 1, 1);
                }

                if (_student.Interact1Timer > 2)
                {

                    ScreenManager.AddScreen(new Minigame1(), player);
                    _student.Interact1Timer = 0;
                    _student.action = Action.Idle;
                    _student.CanInteract1 = false;

                }
                if (_student.Interact2Timer > 2)
                {

                    ScreenManager.AddScreen(new Minigame2(Door), player);
                    _student.Interact2Timer = 0;
                    _student.action = Action.Idle;
                    _student.CanInteract2 = false;

                }
                if(_student.Bounds.CollidesWith(pyBook.Bounds))
                {
                    pyBook.Collected = true;
                    
                    pyBook.Bounds = new BoundingRectangle(-100000, -100000, 1, 1);
                }

                if(_student.Position.X > 1090)
                {
                    LoadingScreen.Load(ScreenManager, false, player, new MVPCompleteScreen());
                }
                //if (PythonCodeCollected)
                //{
                //    ScreenManager.RemoveScreen(Minigame1());
                //}

            }
        }

        public override void Draw(GameTime gameTime)
        {
            float playerX = MathHelper.Clamp(_student.Position.X, 300, 630);
            float offset = 300 - playerX;
            Matrix transform;

            // Matrix transform;

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;


            transform = Matrix.CreateTranslation(offset, 0, 0);

            spriteBatch.Begin(transformMatrix: transform);

            //transform = Matrix.CreateTranslation(offset, 0, 0);
            //_fireworks.Transform = transform;
            // spriteBatch.Begin(transformMatrix: transform);

            // spriteBatch.Begin();
            spriteBatch.Draw(_compLab, new Rectangle(0, 0, 1150, 500), Color.White);
            foreach (var c in computers)
            {
                c.Draw(gameTime, spriteBatch);
            }
            if (!pyBook.Collected) pyBook.Draw(gameTime, spriteBatch);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Left, pyBook.Bounds.Top), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Left, pyBook.Bounds.Bottom), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Right, pyBook.Bounds.Top), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Right, pyBook.Bounds.Bottom), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            Door.Draw(gameTime, spriteBatch);
            //spriteBatch.Draw(_level, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0f);
            _student.Draw(gameTime, spriteBatch);
            //spriteBatch.Draw(circle, new Vector2(_student.Bounds.Left, _student.Bounds.Bottom), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(_student.Bounds.Left, _student.Bounds.Top), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(_student.Bounds.Right, _student.Bounds.Bottom), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(_student.Bounds.Right, _student.Bounds.Top), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(_student.FeetBounds.Left, _student.FeetBounds.Bottom), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(_student.FeetBounds.Left, _student.FeetBounds.Top), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(_student.FeetBounds.Right, _student.FeetBounds.Bottom), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(_student.FeetBounds.Right, _student.FeetBounds.Top), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(Door.Bounds.Left, Door.Bounds.Bottom), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(Door.Bounds.Left, Door.Bounds.Top), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(Door.Bounds.Right, Door.Bounds.Bottom), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(Door.Bounds.Right, Door.Bounds.Top), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            ////spriteBatch.Draw(_overlay, new Rectangle(0, 1, 1150, 500), Color.White);
            //foreach (var c in _boundaries)
            //{
            //    spriteBatch.Draw(circle, new Vector2(c.Left, c.Bottom), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //    spriteBatch.Draw(circle, new Vector2(c.Left, c.Top), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //    spriteBatch.Draw(circle, new Vector2(c.Right, c.Bottom), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //    spriteBatch.Draw(circle, new Vector2(c.Right, c.Top), null, Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //}
            //spriteBatch.Draw(circle, cir.Center, Color.Orange);
            //spriteBatch.Draw(circle, cir2.Center, Color.Orange);
            spriteBatch.End();


            //spriteBatch.Begin();




            //spriteBatch.Draw(circle, _platforms.Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            //spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
