using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using HackersDayOut.StateManagement;
using SharpDX.Direct3D9;

namespace HackersDayOut
{
    public enum State
    {
        Normal = 0,
        Pressed = 1,
    }
    public class Button: GameScreen
    {
        //public Rectangle Bounds { get; private set; }
        private Texture2D _texture;
        

        private double _animationTimer;
        private short _animationFrame;

        private State _state;
        private bool _afterPressed = false;

        public bool IsPressed = false;

        public Vector2 Position;


        public Button(Vector2 position)
        {
            this.Position = position;

        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprite_button");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch /*bool IsSelected*/)
        {
            var source = new Rectangle(_animationFrame * 284, 0, 284, 138);
            if(IsPressed)
            {
                _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if(_animationTimer > 0.2f)
                {
                    _animationFrame++;
                    if (_animationFrame > 1)
                    {
                        _animationFrame = 0;
                        _afterPressed = true;
                    }
                    _animationTimer -= 0.2f;
                }
            }
            if(_afterPressed)
            {
                IsPressed = false;
            }
            //var color = IsSelected ? Color.LightSkyBlue : Color.White; 
            spriteBatch.Draw(_texture, Position, source, Color.White, 0f, Position, 0.35f, SpriteEffects.None, 0);
        }
    }
}
