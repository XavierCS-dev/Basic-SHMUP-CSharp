using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SHMUP
{
    public class Player
    {
        private int health;
        private double speed;
        private int damage;
        private int BulletPause;
        private int BulletCountdown;
        public bool isAlive = true;
        // deathFrames decremented until animation complete
        private int deathFrames = 7;
        // Death Animation Completion Flag
        private bool deathDone = false;
        public int score = 0;
        public int Health
        {
            get { return health; }
        }
        public double Speed
        {
            get { return speed; }
        }
        public int Damage
        {
            get { return damage; }
        }
        public Rectangle TextureSource { get; set; }

        public Rectangle Position;

        public Player(int health = 5, double speed = 1, int damage = 1)
        {
            this.health = health;
            this.speed = speed;
            this.damage = damage;
            Position.X = 200;
            Position.Y = 500;
            Position.Width = 50;
            Position.Height = 50;
            BulletPause = 15;
            BulletCountdown = 0;
        }

        public void Reset(int health = 5, double speed = 1, int damage = 1)
        {
            this.health = health;
            this.speed = speed;
            this.damage = damage;
            Position.X = 200;
            Position.Y = 500;
            Position.Width = 50;
            Position.Height = 50;
            BulletPause = 15;
            BulletCountdown = 0;
            isAlive = true;
            deathFrames = 7;
            deathDone = false;
            score = 0;
        }

        public void Shoot(Texture2D playerBeam)
        {
            if (BulletCountdown <= 0)
            {
                // Creatin Two bullet streams
                Game1.playerBullets.Add(new Bullet(new Rectangle(Position.X - 5, Position.Y - 5, playerBeam.Width / 2, playerBeam.Height / 2), new Rectangle(0, 0, playerBeam.Width, playerBeam.Height), 1));
                Game1.playerBullets.Add(new Bullet(new Rectangle(Position.X + Position.Width - 20, Position.Y - 5, playerBeam.Width / 2, playerBeam.Height / 2), new Rectangle(0, 0, playerBeam.Width, playerBeam.Height), 1));
                // Reset bullet timings
                BulletCountdown = BulletPause;
            }
            BulletCountdown -= 1;     
        }

        public void Death()
        {
            Game1.resetNeeded = true;
            deathDone = true;
        }

        public bool Update(KeyboardState newState, KeyboardState oldStates, Texture2D playerBeam, List<Bullet> enemyBullets)
        {
            // signal death animation complete and remove player
            if (!isAlive && deathDone)
            {
                return false;
            }
            // player is dead but death animation not complete
            else if (!isAlive)
            {
                Death();
                return true;
            }
            // movement controls
            if (newState.IsKeyDown(Keys.W))
            {
                Position.Y -= (int)(10 * speed);
            }
            if (newState.IsKeyDown(Keys.A))
            {
                Position.X -= (int)(10 * speed);
            }
            if (newState.IsKeyDown(Keys.S))
            {
                Position.Y += (int)(10 * speed);
            }
            if (newState.IsKeyDown(Keys.D))
            {
                Position.X += (int)(10 * speed);
            }
            // shooting controls
            if (newState.IsKeyDown(Keys.Space))
            {
                Shoot(playerBeam);
            }

            if (health <= 0)
            {
                health = 0;
                isAlive = false;
            }

            return true;
        }

        public void reduceHealth(int amt)
        {
            health -= amt;
        }

        // Making draw method static for consistancy with other classes
        public static void Draw(SpriteBatch _spriteBatch, Player player, Texture2D texture)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, player.Position, player.TextureSource, Color.White);
            _spriteBatch.End();
        }

    }
}
