using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace SHMUP
{
    public class Bullet
    {
        public Rectangle Position;
        // Snapshot window of texture
        public Rectangle TextureSource { get; set; }
        public bool isAlive = true;
        // Death Animation Completion Flag
        public bool deathDone = false;
        private double speed;
        public double Speed
        {
            get { return speed; }
        }
        public Bullet(Rectangle position, Rectangle textureSource, double speed)
        {
            Position = position;
            this.speed = speed;
            Position = position;
            TextureSource = textureSource;
        }

        public static void UpdateBadBullets(GraphicsDeviceManager _graphics, ref List<Bullet> bullets, ref Player player)
        {
            List<int> bulletsToKill = new List<int>();
            List<int> bulletsToAnim = new List<int>();
            int itemPos = 0;

            foreach(Bullet bullet in bullets)
            {

                if (!bullet.isAlive && bullet.deathDone)
                {
                    bulletsToKill.Add(itemPos);
                }
                else if(!bullet.isAlive)
                {
                    bullet.Death();
                }
                // instantly kill bullets if they go off screen and skip
                // death animation
                if (bullet.Position.Y < 0 || bullet.Position.Y > _graphics.PreferredBackBufferHeight)
                {
                    bulletsToKill.Add(itemPos);
                } else if (bullet.Position.Intersects(player.Position))
                {
                    player.reduceHealth(1);
                    bulletsToKill.Add(itemPos);
                }
                ++itemPos;
                bullet.Position.Y += (int)(30 * bullet.speed);

            }
            
            for(int i = bulletsToKill.Count - 1; i >=0; --i)
            {
                bullets.RemoveAt(bulletsToKill[i]);
            }

            // going through the list backwards to stop it reordering

            
        }

        public static void UpdateGoodBullets(GraphicsDeviceManager _graphics, ref List<Bullet> bullets, Player player)
        {
            List<int> bulletsToKill = new List<int>();
            List<int> bulletsToAnim = new List<int>();
            int itemPos = 0;

            foreach (Bullet bullet in bullets)
            {

                if (!bullet.isAlive && bullet.deathDone)
                {
                    bulletsToKill.Add(itemPos);
                }
                else if (!bullet.isAlive)
                {
                    bullet.Death();
                }
                // instantly kill bullets if they go off screen and skip
                // death animation
                if (bullet.Position.Y < 0 || bullet.Position.Y > _graphics.PreferredBackBufferHeight)
                {
                    bulletsToKill.Add(itemPos);
                }
                foreach(Enemy enemy in Levels.enemyList)
                {
                    if(bullet.Position.Intersects(enemy.Position))
                    {
                        enemy.reduceHealth(1);
                        player.score += 50;
                        if (!bulletsToKill.Contains(itemPos))
                        {
                           bulletsToKill.Add(itemPos);
                        }
                       
                    }
                }
                ++itemPos;
                bullet.Position.Y -= (int)(30 * bullet.speed);

            }

            for (int i = bulletsToKill.Count - 1; i >= 0; --i)
            {
                bullets.RemoveAt(bulletsToKill[i]);
            }

            // going through the list backwards to stop it reordering


        }

        public void Death()
        {
            deathDone = true;
        }

        // static draw method to batch rendering of bullets to reduce draw calls
        public static void Draw(SpriteBatch _spriteBatch, List<Bullet> bullets, Texture2D texture)
        {
            _spriteBatch.Begin();
            foreach (Bullet bullet in bullets)
            {
                _spriteBatch.Draw(texture, bullet.Position, bullet.TextureSource, Color.White);
            }
            _spriteBatch.End();
        }
    }
}
