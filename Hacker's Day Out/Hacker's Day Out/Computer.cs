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
        private int _comp;

        public Computer(Vector2 Pos, int Comp)
        {
            pos = Pos;
            _comp = Comp;
         }


        public void LoadContent(ContentManager content)
        {
            switch(_comp)
            {
                case (1):
                    _texture = content.Load<Texture2D>("Sprite_computer");
                    break;
                case (2):
                    _texture = content.Load<Texture2D>("Sprite_computer_Java");
                    break;
                case (3):
                    _texture = content.Load<Texture2D>("Sprite_computer_C");
                    break;
                case (4):
                    _texture = content.Load<Texture2D>("Sprite_computer_CSharp");
                    break;
                default:
                    break;
            }
           


        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, pos, new Rectangle(0, 0, 400, 302), Color.White, 0f, new Vector2(200, 152), 0.45f, SpriteEffects.None, 0);
        }
    }
}
