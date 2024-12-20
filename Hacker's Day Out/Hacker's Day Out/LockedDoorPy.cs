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
        private int _alignment;
        private int _doorlock;


        public float Scale;
        public BoundingRectangle Bounds;
        public doorState State;

        public LockedDoorPy(int Alignment, int DoorLock, Vector2 Position, float s, BoundingRectangle bounds, bool flipped)
        {
            _alignment = Alignment;
            _pos = Position;
            Scale = s;
            Bounds = bounds;
            _flipped = flipped;
            _doorlock = DoorLock;
        }

        public void LoadContent(ContentManager content)
        {
            switch (_doorlock)
            {
                case 1:
                    if (_alignment == 1) _texture = content.Load<Texture2D>("Sprite_reglockeddoor");
                    else _texture = content.Load<Texture2D>("Sprite_pylockeddoor2");
                    break;
                case 2:
                    if (_alignment == 1) _texture = content.Load<Texture2D>("Sprite_Javalockeddoor");
                    else _texture = content.Load<Texture2D>("Sprite_Javalockeddoor2");
                    break;
                case 3:
                    if (_alignment == 1) _texture = content.Load<Texture2D>("Sprite_Clockeddoor");
                    else _texture = content.Load<Texture2D>("Sprite_Clockeddoor2");
                    break;
                case 4:
                    if (_alignment == 1) _texture = content.Load<Texture2D>("Sprite_CSharplockeddoor");
                    else _texture = content.Load<Texture2D>("Sprite_CSharplockeddoor2");
                    break;

                default:
                    break;
            }
            
        }

        public void Update(GameTime gameTime)
        {
            
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
            if (_alignment ==1)spriteBatch.Draw(_texture, _pos, source, Color.White, 0.00f, new Vector2(0, 0), Scale, spriteEffects, 0);
            else spriteBatch.Draw(_texture, _pos, source, Color.White, 0.00f, new Vector2(0, 0), Scale, spriteEffects, 0);
        }
    }
}
