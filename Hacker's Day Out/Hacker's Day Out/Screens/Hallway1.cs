﻿using System;
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
using Microsoft.VisualBasic.Devices;
using System.IO;


namespace HackersDayOut.Screens
{
    public class Hallway1: GameScreen
    {
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        private Texture2D _Halls1;

        private Student _student; 

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private BoundingRectangle[] _boundaries;

        private Random random = new Random();

        public Texture2D circle;

        public BoundingCircle cir;

        public static LockedDoorPy ObjDoor = new LockedDoorPy(2, 4, new Vector2(344, 87), 1.2f, new BoundingRectangle(305, 87, 200, 250), false);

        public LockedDoorPy Door;

        public int RandInt;

        public Hallway1(Vector2 sp)
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
            _Halls1 = _content.Load<Texture2D>("Hallway1");
            ObjDoor.LoadContent(_content);
            circle = _content.Load<Texture2D>("circle");


            _boundaries = new BoundingRectangle[]
                {
                    new BoundingRectangle(50, -100, 40, 400),
                    new BoundingRectangle(0, 450, 40, 1000),
                    new BoundingRectangle(0, 0, 315, 354),
                    new BoundingRectangle(490, 0, 395, 354),
                    new BoundingRectangle(845, 0, 435, 340),
                    
                    new BoundingRectangle(1000, 360, 120, 50),
                    new BoundingRectangle(0, 500, 1090, 70),
                  


                };

            cir = new BoundingCircle(new Vector2(380, 180), 10f);

            Door = new LockedDoorPy(1, 1, new Vector2(-30, 100), 1.2f, new BoundingRectangle(-9999, -9999, 1, 1), true);
            Door.LoadContent(_content);
            Door.State = doorState.Open;

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
            if(ScreenManager.Door1Opened)
            {
                ObjDoor.State = doorState.Open;
                string text = File.ReadAllText("progress.txt");
                string replaced = text.Replace("Door1Locked", "Door1Unlocked");
                File.WriteAllText("progress.txt", replaced);
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
                if (_student.Position.X < 70)
                {
                    string text = File.ReadAllText("progress.txt");
                    string replaced = text.Replace("RoomHallway1", "RoomCompLab");
                    File.WriteAllText("progress.txt", replaced);
                    RoomTransfer rt1 = new RoomTransfer(ScreenManager, this, new ComputerRoom(new Vector2(985, 210)), ControllingPlayer);
                }
                if(_student.Position.X >= 1090 && _student.Position.Y > 253)
                {
                    string text = File.ReadAllText("progress.txt");
                    string replaced = text.Replace("RoomHallway1", "RoomHallway2");
                    File.WriteAllText("progress.txt", replaced);
                    RoomTransfer rt2 = new RoomTransfer(ScreenManager, this, new Hallway2(new Vector2(1035, 215)), ControllingPlayer);
                }
                if (_student.Position.X > 1014 && _student.Position.Y < 205)
                {
                    string text = File.ReadAllText("progress.txt");
                    string replaced = text.Replace("RoomHallway1", "RoomHallway3");
                    File.WriteAllText("progress.txt", replaced);
                    RoomTransfer rt3 = new RoomTransfer(ScreenManager, this, new Hallway3(new Vector2(1035, 255)), ControllingPlayer);
                }

                _student.CollisionHandling(ObjDoor.Bounds);
                if (ObjDoor.State == doorState.Closed && ScreenManager.CSharpCodeCollected)
                {
                    _student.InteractHandlingOne(cir);
                }
                if (_student.Interact1Timer > 2)
                {
                    RandInt = random.Next(1, 3);
                    if (RandInt == 1) ScreenManager.AddScreen(new Minigame2(ObjDoor, 4), player);
                    else ScreenManager.AddScreen(new Minigame4(ObjDoor, 4), player);
                    _student.Interact1Timer = 0;
                    _student.action = Action.Idle;
                    _student.CanInteract1 = false;

                }
                if (ObjDoor.State != doorState.Closed)
                {
                    ObjDoor.Bounds = new BoundingRectangle(305, 187, 60, 200);
                }
                else
                {
                    ObjDoor.Bounds = new BoundingRectangle(305, 87, 200, 250);
                }
                //if (_student.Position.X > 1010 && _student.Position.Y < 185 )
                //{
                //    RoomTransfer rt1 = new RoomTransfer(ScreenManager, this, new ComputerRoom(new Vector2(985, 210)), ControllingPlayer);
                //}

                //if (PythonCodeCollected)
                //{
                //    ScreenManager.RemoveScreen(Minigame1());
                //}
                if(_student.Position.Y < 80)
                {
                    ScreenManager.AddScreen(new Ending(), ControllingPlayer);
                }

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
            spriteBatch.Draw(_Halls1, new Rectangle(0, -20, 1150, 520), Color.White);
            ObjDoor.Draw(gameTime, spriteBatch);
            Door.Draw(gameTime, spriteBatch);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Left, pyBook.Bounds.Top), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Left, pyBook.Bounds.Bottom), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Right, pyBook.Bounds.Top), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(pyBook.Bounds.Right, pyBook.Bounds.Bottom), null, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

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
            //spriteBatch.Draw(circle, new Vector2(ObjDoor.Bounds.Left, ObjDoor.Bounds.Bottom), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(ObjDoor.Bounds.Left, ObjDoor.Bounds.Top), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(ObjDoor.Bounds.Right, ObjDoor.Bounds.Bottom), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(circle, new Vector2(ObjDoor.Bounds.Right, ObjDoor.Bounds.Top), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

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
