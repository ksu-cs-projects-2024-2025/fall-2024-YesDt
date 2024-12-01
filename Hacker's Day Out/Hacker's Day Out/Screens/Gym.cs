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
using System.IO;

namespace HackersDayOut.Screens
{
    public class Gym : GameScreen
    {
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        private Texture2D _gym;

        private Texture2D _hoops;
        
        private Student _student;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private BoundingRectangle[] _boundaries;

        private Random random = new Random();

        public Texture2D circle;

        public Vector2 SpawnPosition;

        public BoundingCircle cir;

        //public BoundingCircle cir2;

        //public Computer[] computers;

        //public LockedDoorPy Door;

        // public bool ComputerCode = false; 

       public static CBook cBook;

       public static LockedDoorPy ObjDoor6 = new LockedDoorPy(2, 2, new Vector2(979, 133), 1.22f, new BoundingRectangle(935, 138, 350, 240), false);

        public int RandInt;


        public Gym(Vector2 sp)
        {
            _student = new Student(sp);
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
            _gym = _content.Load<Texture2D>("Sprite_Gym");
            _hoops = _content.Load<Texture2D>("Sprite_Gym_Hoops");

            circle = _content.Load<Texture2D>("circle");


            _boundaries = new BoundingRectangle[]
                {
                    new BoundingRectangle(-10, 350, 100, 90),
                    new BoundingRectangle(-10, 580, 100, 1000),
                    new BoundingRectangle(-50, 300, 1020, 80),
                    new BoundingRectangle(-40, 254, 2000, 80),
                    new BoundingRectangle(1140, 300, 1010, 80),
                    new BoundingRectangle(1900, 224, 40, 1000),
                    //new BoundingRectangle(1040, 400, 40, 400),
                    new BoundingRectangle(0, 1300, 5090, 70),

                };

            cir = new BoundingCircle(new Vector2(1045, 200), 10f);
            //cir2 = new BoundingCircle(new Vector2(1050, 280), 50f);

            //Door = new LockedDoorPy(new Vector2(1015, 250), new BoundingRectangle(1050, 260, 140, 130), false);
            ObjDoor6.LoadContent(_content);
            if (!ScreenManager.CBookCollected)
            {
                cBook = new CBook(new Vector2(1000, 290));
                cBook.LoadContent(_content);
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
                _student.CollisionHandling(ObjDoor6.Bounds);
                //if(ScreenManager.JavaBookCollected && !ScreenManager.JavaCodeCollected)
                //{
                //    _student.InteractHandlingOne(cir);
                //}

                if (ObjDoor6.State == doorState.Closed && ScreenManager.PythonCodeCollected)
                {
                    _student.InteractHandlingOne(cir);
                }
                if (ObjDoor6.State != doorState.Closed)
                {
                    string text = File.ReadAllText("progress.txt");
                    string replaced = text.Replace("Door6Locked", "Door6Unlocked");
                    File.WriteAllText("progress.txt", replaced);
                    ObjDoor6.Bounds = new BoundingRectangle(-10000, -100000, 1, 1);
                }


                if (_student.Interact1Timer > 2)
                {
                    RandInt = random.Next(1, 3);
                    if (RandInt == 1) ScreenManager.AddScreen(new Minigame2(ObjDoor6, 2), player);
                    else ScreenManager.AddScreen(new Minigame4(ObjDoor6, 2), player);
                    _student.Interact1Timer = 0;
                    _student.action = Action.Idle;
                    _student.CanInteract1 = false;

                }
                if (_student.FeetBounds.CollidesWith(cBook.Bounds))
                {
                    ScreenManager.CBookCollected = true;

                    cBook.Bounds = new BoundingRectangle(-100000, -100000, 1, 1);
                }

                if (_student.Position.X < 65)
                {
                    string text = File.ReadAllText("progress.txt");
                    string replaced = text.Replace("RoomGym", "RoomHallway2");
                    File.WriteAllText("progress.txt", replaced);
                    RoomTransfer rt1 = new RoomTransfer(ScreenManager, this, new Hallway2(new Vector2(1900, 240)), ControllingPlayer);
                }
                //if (PythonCodeCollected)
                //{
                //    ScreenManager.RemoveScreen(Minigame1());
                //}

            }
        }

        public override void Draw(GameTime gameTime)
        {
            float playerX = MathHelper.Clamp(_student.Position.X, 300, 1480);
            float offset = 300 - playerX;
            float playerY = MathHelper.Clamp(_student.Position.Y, 200, 800);
            float offsetY = 100 - playerY;
            Matrix transform;

            // Matrix transform;

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;


            transform = Matrix.CreateTranslation(offset, offsetY, 0);

            spriteBatch.Begin(transformMatrix: transform);

            //transform = Matrix.CreateTranslation(offset, 0, 0);
            //_fireworks.Transform = transform;
            // spriteBatch.Begin(transformMatrix: transform);

            // spriteBatch.Begin();
            spriteBatch.Draw(_gym, new Rectangle(0, 0, 2000, 1200), Color.White);

            if (!ScreenManager.CBookCollected) cBook.Draw(gameTime, spriteBatch);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Left, pyBook.Bounds.Top), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Left, pyBook.Bounds.Bottom), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Right, pyBook.Bounds.Top), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Right, pyBook.Bounds.Bottom), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            ObjDoor6.Draw(gameTime, spriteBatch);
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
            //spriteBatch.Draw(circle, new Vector2(ObjDoor6.Bounds.Left, ObjDoor6.Bounds.Bottom), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(ObjDoor6.Bounds.Left, ObjDoor6.Bounds.Top), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(ObjDoor6.Bounds.Right, ObjDoor6.Bounds.Bottom), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(ObjDoor6.Bounds.Right, ObjDoor6.Bounds.Top), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);


            spriteBatch.Draw(_hoops, new Rectangle(0, 0, 2000, 1200), Color.White);
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
