﻿using HackersDayOut.Collisions;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using HackersDayOut.StateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HackersDayOut
{
    /// <summary>
    /// States the main character will be in
    /// </summary>
    public enum Action
    {
        Idle = 0,
        Walking = 1,
        InteractingSide = 2,
        InteractingUp = 3,

    }

    public class Student
    {
        #region privateFields
        private Texture2D _texture;

        private Texture2D _interactButton;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _priorKeyboardState;

        private BoundingRectangle _bounds;

        private BoundingRectangle _feet;

        private double _animationTimer;


        private short _animationFrame;

        private Vector2 _xDirection;
        private Vector2 _yDirection;

        private bool _sayingHi = false;
        #endregion

        #region publicFields

        public Vector2 Position = new Vector2();

        public bool Flipped;

        public bool CanInteract1 = false;

        public bool CanInteract2 = false;

        public double Interact1Timer;

        public double Interact2Timer;

        public Action action;

        public short AnimationFrame => _animationFrame;

        public BoundingRectangle Bounds => _bounds;

        public BoundingRectangle FeetBounds => _feet;


        public BoundingRectangle rectangle;

        public BoundingCircle circle;
        #endregion

        public Student(Vector2 pos)
        {
            Position = pos;
            _bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y + 32), 64, 196);
            _feet = new BoundingRectangle(new(_bounds.X, _bounds.Y + 16), 64, 32);
        }

        /// <summary>
        /// Loads the Main character sprite
        /// </summary>
        /// <param name="content">ContentManager</param>
        public void LoadContent(ContentManager content)
        {
            if(ScreenManager.WearingCostume) _texture = content.Load<Texture2D>("Sprite_student2");
            else _texture = content.Load<Texture2D>("Sprite_student");
            _interactButton = content.Load<Texture2D>("Sprite_interact");


        }

        public void Update(GameTime gameTime)
        {
            _xDirection = new Vector2(300 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            _yDirection = new Vector2(0, 300 * (float)gameTime.ElapsedGameTime.TotalSeconds);


            _priorKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            CollisionHandling(rectangle);

            if (action != Action.InteractingUp && action != Action.InteractingSide)
            {
                if (_currentKeyboardState.IsKeyDown(Keys.A) ||
               _currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    Position += -_xDirection;
                    action = Action.Walking;
                    Flipped = true;
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D) ||
                    _currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    Position += _xDirection;
                    action = Action.Walking;
                    Flipped = false;
                }
                if (_currentKeyboardState.IsKeyDown(Keys.W) ||
                    _currentKeyboardState.IsKeyDown(Keys.Up))
                {
                    Position -= _yDirection;
                    action = Action.Walking;
                }
                if (_currentKeyboardState.IsKeyDown(Keys.S) ||
                    _currentKeyboardState.IsKeyDown(Keys.Down))
                {
                    Position += _yDirection;
                    action = Action.Walking;
                }

                if ((CanInteract1) && _currentKeyboardState.IsKeyDown(Keys.E))
                {
                    action = Action.InteractingUp;
                    _animationTimer = 0;
                    _animationFrame = 0;
                }

                if ((CanInteract2) && _currentKeyboardState.IsKeyDown(Keys.E))
                {
                    action = Action.InteractingSide;
                    _animationTimer = 0;
                    _animationFrame = 0;
                }
                if (!(_currentKeyboardState.IsKeyDown(Keys.A) ||
                   _currentKeyboardState.IsKeyDown(Keys.Left)) &&
                   !(_currentKeyboardState.IsKeyDown(Keys.D) ||
                   _currentKeyboardState.IsKeyDown(Keys.Right))
                    &&
                   !(_currentKeyboardState.IsKeyDown(Keys.W) ||
                   _currentKeyboardState.IsKeyDown(Keys.Up))
                    &&
                   !(_currentKeyboardState.IsKeyDown(Keys.S) ||
                   _currentKeyboardState.IsKeyDown(Keys.Down))
                   &&
                   (!_currentKeyboardState.IsKeyDown(Keys.H))
                   && (action != Action.InteractingSide && action != Action.InteractingUp)
                   )
                {
                    action = Action.Idle;
                }
            }
            if (action == Action.InteractingUp)
            {
                Interact1Timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (Interact1Timer > 3)
                {
                    action = Action.Idle;
                    Interact1Timer = 0;
                }

            }
            if (action == Action.InteractingSide)
            {
                Interact2Timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (Interact2Timer > 3)
                {
                    action = Action.Idle;
                    Interact2Timer = 0;
                }

            }



            _bounds.X = Position.X - 48;
            _bounds.Y = Position.Y - 48;
            _feet.X = _bounds.X;
            _feet.Y = _bounds.Bottom + 8;
        }
        public void CollisionHandling(BoundingRectangle rect)
        {
            rectangle = rect;


            if (_feet.CollidesWith(rect))
            {

                if (_bounds.Bottom < rect.Top && _bounds.Left > rect.Left && _bounds.Right < rect.Right) Position.Y -= 5;
                else if (_bounds.Top < rect.Bottom && _bounds.Left > rect.Left && _bounds.Right < rect.Right)
                {
                    Position.Y += 5;
                }
                else if (_bounds.Left < rect.Right && _bounds.Right > rect.Right && _bounds.Bottom > rect.Top && _bounds.Top < rect.Bottom)
                {
                    Position.X += 5;

                }
                else if (_bounds.Right > rect.Left && _bounds.Left < rect.Left && _bounds.Bottom > rect.Top && _bounds.Top < rect.Bottom)
                {
                    Position.X -= 5;
                }
            }


        }
        public void InteractHandlingOne(BoundingCircle cir)
        {
            circle = cir;
            if (_bounds.CollidesWith(cir))
            {
                CanInteract1 = true;
            }
            else
            {
                CanInteract1 = false;
            }
        }

        public void InteractHandlingTwo(BoundingCircle cir)
        {
            circle = cir;
            if (_bounds.CollidesWith(cir))
            {
                CanInteract2 = true;
            }
            else
            {
                CanInteract2 = false;
            }
        }
        /// <summary>
        /// Draws the main character
        /// </summary>
        /// <param name="gameTime">The real time elapsed in the game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            SpriteEffects spriteEffects = (Flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


            switch ((int)action)
            {
                case 1:

                    _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (_animationTimer > 0.1)
                    {
                        _animationFrame++;
                        if (_animationFrame > 7)
                        {
                            _animationFrame = 0;

                        }
                        _animationTimer -= 0.1;
                    }
                    break;

                case 2:
                    _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (_animationTimer > 0.15 && _animationFrame < 6)
                    {
                        _animationFrame++;
                        _animationTimer -= 0.15;
                    }
                    break;

                case 3:
                    _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (_animationTimer > 0.15 && _animationFrame < 6)
                    {
                        _animationFrame++;
                        _animationTimer -= 0.15;
                    }
                    break;

                default:
                    _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (_animationTimer > 0.2 && _animationFrame != 3)
                    {
                        _animationFrame++;
                        if (_animationFrame > 7)
                        {
                            _animationFrame = 0;

                        }
                        _animationTimer -= 0.2;
                    }
                    if (_animationFrame == 3)
                    {
                        if (_animationTimer > 0.8)
                        {
                            _animationFrame++;
                            _animationTimer = 0;
                        }
                    }
                    break;

            }


            var source = new Rectangle(_animationFrame * 149, (int)action * 386, 147, 385);
            if (CanInteract1 || CanInteract2) spriteBatch.Draw(_interactButton, new Rectangle((int)Position.X + 5, (int)Position.Y - 120, 90, 60), Color.White);
            spriteBatch.Draw(_texture, Position, source, Color.White, 0f, new Vector2(80, 120), 0.75f, spriteEffects, 0); //new Vector2(76, 192)


        }
    }
}
