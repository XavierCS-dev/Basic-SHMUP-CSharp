using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace SHMUP
{
    class Levels
    {
        public static List<Enemy> enemyList = new List<Enemy>();
        // currentLevel value of 9999 indicates all levels completed
        public static int CurrentLevel = 0;
        // enemies still to be spawned
        private static int remainingEnemies = 0;
        // damage enemies take before dying
        private static int enemyToughness = 0;
        private static int enemyBulletPause = 0;
        private static int enemySpeed = 0;
        // The higher this value, the greater the difficulty, increases each level
        private static int maxEnemiesOnScreen = 0;
        public static int currentEnemiesOnScreen = 0;
        private static Random numGen = new Random();


        public static void Reset(Player player, Texture2D playerShip)
        {
            enemyList = new List<Enemy>();
            CurrentLevel = 0;
            remainingEnemies = 0;
            enemyToughness = 0;
            enemyBulletPause = 0;
            enemySpeed = 0;
            maxEnemiesOnScreen = 0;
            currentEnemiesOnScreen = 0;
            player.Reset();
            player.TextureSource = new Rectangle(0, 0, playerShip.Width, playerShip.Height);
            Game1.enemyBullets = new List<Bullet>();
            Game1.playerBullets = new List<Bullet>();
            nextLevel();
            Game1.resetNeeded = false;
        }


        public static void init()
        {
            nextLevel();
        }



        public static void Update(ref Texture2D enemyTexAtlas, GraphicsDeviceManager _graphics)
        {
            if ((remainingEnemies == 0) && (currentEnemiesOnScreen == 0) && !Game1.gameWon)
            {
                nextLevel();
            }

            if ((remainingEnemies > 0) && (currentEnemiesOnScreen < maxEnemiesOnScreen))
            {
                ++currentEnemiesOnScreen;
                --remainingEnemies;
                Rectangle texSource = Enemy.GetTextureSource(enemyTexAtlas, numGen.Next(0, 5));
                enemyList.Add(new Enemy(texSource, new Rectangle(numGen.Next(0, _graphics.PreferredBackBufferWidth - 50), 0, texSource.Width, texSource.Height), enemyToughness, numGen.Next(1, enemySpeed), 1, enemyBulletPause));
            }
        }

        public static void nextLevel()
        {
            ++CurrentLevel;
            switch (CurrentLevel)
            {
                case 1:
                    levelOne();
                    break;
                case 2:
                    levelTwo();
                    break;
                case 3:
                    levelThree();
                    break;
                case 4:
                    levelFour();
                    break;
                case 5:
                    levelFive();
                    break;
                case 6:
                    GameOver();
                    break;
            }
        }

        public static void levelOne()
        {
            remainingEnemies = 5;
            enemyToughness = 3;
            maxEnemiesOnScreen = 5;
            enemyBulletPause = 100;
            enemySpeed = 5;
        }

        public static void levelTwo()
        {
            remainingEnemies = 15;
            enemyToughness = 3;
            maxEnemiesOnScreen = 7;
            enemyBulletPause = 100;
            enemySpeed = 5;
        }

        public static void levelThree()
        {
            remainingEnemies = 20;
            enemyToughness = 3;
            maxEnemiesOnScreen = 10;
            enemyBulletPause = 100;
            enemySpeed = 5;
        }
        public static void levelFour()
        {
            remainingEnemies = 25;
            enemyToughness = 4;
            maxEnemiesOnScreen = 10;
            enemyBulletPause = 75;
            enemySpeed = 5;
        }

        public static void levelFive()
        {
            remainingEnemies = 50;
            enemyToughness = 5;
            maxEnemiesOnScreen = 15;
            enemyBulletPause = 50;
            enemySpeed = 2;
        }

        public static void GameOver()
        {
            Game1.gameWon = true;
        }
    }
}
