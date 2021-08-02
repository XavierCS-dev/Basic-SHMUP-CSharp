using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace SHMUP
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D playerShip;
        private Texture2D enemyAtlas;
        private int enemyAtlasShipHeight;
        private int enemyAtlasShipWidth;
        private Texture2D fastBeam;
        private Texture2D playerBeam;
        private Texture2D enemyBeam;
        private Texture2D enemyDeathAtlas;
        private Texture2D playerDeathAtlas;
        private Texture2D asteroid;
        private KeyboardState oldState;
        private Random rand = new Random();
        private SpriteFont font;
        public static List<Bullet> playerBullets = new List<Bullet>();
        public static List<Bullet> enemyBullets = new List<Bullet>();
        public static bool resetNeeded = false;
        public static bool gameWon = false;


        public Player player;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 550;
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.ApplyChanges();
            player = new Player();
            KeyboardState newState = Keyboard.GetState();
            oldState = newState;
            Levels.init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            playerShip = Content.Load<Texture2D>("ship");
            player.TextureSource = new Rectangle(0, 0, playerShip.Width, playerShip.Height);
            enemyAtlas = Content.Load<Texture2D>("enemyShips");
            fastBeam = Content.Load<Texture2D>("fastBeam");
            playerBeam = Content.Load<Texture2D>("playerBeam");
            enemyBeam = Content.Load<Texture2D>("enemyBeam");
            enemyDeathAtlas = Content.Load<Texture2D>("enemyDeath");
            playerDeathAtlas = Content.Load<Texture2D>("playerDeath");
            asteroid = Content.Load<Texture2D>("asteriod");
            font = Content.Load<SpriteFont>("font");
            enemyAtlasShipHeight = enemyAtlas.Height;
            enemyAtlasShipWidth = enemyAtlas.Width / 6;

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (resetNeeded)
            {
                Levels.Reset(player, playerShip);
            } 
            

            // TODO: Add your update logic here
            KeyboardState newState = Keyboard.GetState();
            player.Update(newState, oldState, playerBeam, enemyBullets);

            int enemyPos = 0;
            List<int> enemiesToKill = new List<int>();
            foreach(Enemy enemy in Levels.enemyList)
            {
                if(!enemy.Update(enemyBeam, _graphics))
                {
                    enemiesToKill.Add(enemyPos);
                }
                ++enemyPos;
            }

            for (int i = enemiesToKill.Count - 1; i >= 0; --i)
            {
                Levels.enemyList.RemoveAt(enemiesToKill[i]);
            }


            // 0 means enemy bullet origin
            // 1 means player bullet origin
            Bullet.UpdateBadBullets(_graphics, ref enemyBullets, ref player);
            Bullet.UpdateGoodBullets(_graphics, ref playerBullets, player);

            Levels.Update(ref enemyAtlas, _graphics);

            oldState = newState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            Enemy.Draw(_spriteBatch, Levels.enemyList, enemyAtlas);
            Player.Draw(_spriteBatch, player, playerShip);
            Bullet.Draw(_spriteBatch, playerBullets, playerBeam);
            Bullet.Draw(_spriteBatch, enemyBullets, enemyBeam);
            HUD.Draw(_spriteBatch, font, player, _graphics);
            if (gameWon)
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "  You win!\nGame Over.", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 50, _graphics.PreferredBackBufferHeight / 2 - 50), Color.White);
                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }



    }
}
