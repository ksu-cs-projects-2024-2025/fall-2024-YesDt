using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using HackersDayOut.StateManagement;

namespace HackersDayOut
{
    public class Computer
    {
        private Texture2D _texture;
        private Vector2 pos;

        public Computer(Vector2 Pos)
        {
            pos = Pos;
        }


        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprite_computer");


        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, pos, new Rectangle(0, 0, 400, 302), Color.White, 0f, new Vector2(200, 152), 0.45f, SpriteEffects.None, 0);
        }
    }
}
