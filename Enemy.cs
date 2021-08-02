using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SHMUP
{
    class Enemy
    {
        private static int enemies = 0;
        public static int EnemyCount
        {
            get { return enemies; }
        }

        public Rectangle Position;
        public Rectangle TextureSource { get; set; }

        public int Health { get; set; }
        public int Speed { get; set; }
        public int Damage { get; set; }
        private int BulletPause;
        private int BulletCountdown;
        public bool isAlive = true;
        // Death Animation Completion Flag
        private bool deathDone = false;
        private int slide = 1;
        Random rand = new Random();

        public Enemy(Rectangle textureSource, Rectangle position, int health, int speed, int damage, int bulletPause)
        {
            TextureSource = textureSource;
            Position = position;
            Health = health;
            Speed = speed;
            Damage = damage;
            BulletPause = bulletPause;
            BulletCountdown = bulletPause;
            switch(rand.Next(0,3))
            {
                case 0:
                    slide = 1;
                    break;
                case 2:
                    slide = -1;
                    break;
                case 3:
                    slide = 0;
                    break;
            }
            ++enemies;

        }

        public void Death()
        {
            --Levels.currentEnemiesOnScreen;
            deathDone = true;
        }

        public bool Update(Texture2D enemyBeam, GraphicsDeviceManager _graphics)
        {
            if(!isAlive && deathDone)
            {
                return false;
            }
            else if(!isAlive)
            {
                // start/continue death animation
                Death();
                return true;
            }
            if (Health <= 0)
            {
                isAlive = false;
                return true;
            }

            if (BulletCountdown <= 0)
            {
                shoot(enemyBeam);
                BulletCountdown = BulletPause;
            }

            Position.Y += Speed;
            Position.X += (Speed / 2) * slide;
            if (Position.X > _graphics.PreferredBackBufferWidth + 30)
            {
                switch(rand.Next(0,2))
                {
                    case 0:
                        Position.X = -20;
                        break;
                    default:
                        slide = -1;
                        break;

                }
            }
            else if (Position.X < -30)
            {
                switch (rand.Next(0, 2))
                {
                    case 0:
                        Position.X = _graphics.PreferredBackBufferWidth + 30;
                        break;
                    default:
                        slide = 1;
                        break;

                }
            }

            --BulletCountdown;

            if(Position.Y > _graphics.PreferredBackBufferHeight)
            {
                Position.Y = -20;
            }
            
            return true;
        }

        private void shoot(Texture2D enemyBeam)
        {
            Game1.enemyBullets.Add(new Bullet(new Rectangle(Position.X + 15, Position.Y - 5, enemyBeam.Width / 4, enemyBeam.Height / 4), 
                new Rectangle(0, 0, enemyBeam.Width, enemyBeam.Height), 0.2));
        }


        // Static draw method to batch drawing of enemies to reduce draw calls
        public static void Draw(SpriteBatch _spriteBatch, List<Enemy> enemies, Texture2D texture)
        {
            _spriteBatch.Begin();

            
            foreach(Enemy enemy in enemies)
            {
                _spriteBatch.Draw(texture, enemy.Position, enemy.TextureSource, Color.White);
            }
            

            _spriteBatch.End();
        }

        public void reduceHealth(int amt)
        {
            Health -= amt;
        }

        // Get texture rectangle source from texture atlas
        public static Rectangle GetTextureSource(Texture2D texture, int position)
        {
            Rectangle texSource;
            int oneTex = texture.Width / 6;
            texSource.X = oneTex * position;
            texSource.Y = 0;
            texSource.Width = oneTex;
            texSource.Height = texture.Height;
            return texSource;
        }


    }
}
