﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackersDayOut.Collisions;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace HackersDayOut
{
    public enum doorState
    {
        Closed = 0,
        Opening = 1,
        Open = 2,
    }

    public class LockedDoorPy
    {
        private Texture2D _texture;
        private Vector2 _pos;
        private bool _flipped;
        private double _animationTimer;
        private double _animationFrame;


        public BoundingRectangle Bounds;
        public doorState State;

        public LockedDoorPy(Vector2 Position, BoundingRectangle bounds, bool flipped)
        {
            _pos = Position;
            Bounds = bounds;
            _flipped = flipped;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprite_reglockeddoor");
        }

        public void Update(GameTime gameTime)
        {
            if (State == doorState.Opening || State == doorState.Open)
            {
                Bounds = new BoundingRectangle(-100000, -100000, 1, 1);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle((int)_animationFrame * 209, 0, 208, 360);

            SpriteEffects spriteEffects = (_flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            switch((int)State)
            {
                case 1:
                    _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (_animationTimer > 0.1 && _animationFrame < 9)
                    {
                        _animationFrame++;
                        _animationTimer -= 0.1;
                    }
                    break;
                case 2:
                    _animationFrame = 9;
                    break;
                default:
                    _animationFrame = 0;
                    break;
            }
            spriteBatch.Draw(_texture, _pos, source, Color.White, 0.00f, new Vector2(104, 160), 1.0f, spriteEffects, 0);
        }
    }
}