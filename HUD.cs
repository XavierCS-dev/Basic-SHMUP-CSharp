using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace SHMUP
{
    class HUD
    {
        public static int Health
        {
            get { return Health; }
            set { Health = value; }
        }

        public static void Draw(SpriteBatch _spritebatch, SpriteFont font, Player player, GraphicsDeviceManager _graphics)
        {
            _spritebatch.Begin();

            _spritebatch.DrawString(font, $"Score: {player.score}", new Vector2(0, 0), Color.White);
            _spritebatch.DrawString(font, $"Health: {player.Health}", new Vector2(_graphics.PreferredBackBufferWidth - 100, 0), Color.White);
            _spritebatch.DrawString(font, $"Level: {Levels.CurrentLevel}", new Vector2(0, _graphics.PreferredBackBufferHeight - 27), Color.White);

            _spritebatch.End();
        }

    }
}
