using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace JointProject2C00172382
{
    class Player
    {
        //textures
        Texture2D playerLeft1;
        Texture2D playerLeft2;
        Texture2D playerLeft3;
        Texture2D playerLeft4;
        Texture2D playerRight1;
        Texture2D playerRight2;
        Texture2D playerRight3;
        Texture2D playerRight4;
        Texture2D playerFront1;
        Texture2D playerFront2;
        Texture2D playerFront3;
        Texture2D playerBullet;
        Texture2D playerBack1;
        Texture2D playerBack2;
        Texture2D playerBack3;
        //general player info
        int score;
        int hostagesSaved;
        int hostagesShot;
        int health;
        int lives;
        public string name;
        //player position
        public Vector2 playerPos;
        //counter for changing the sprite(animation)
        int spriteLeftCounter = 0;
        int spriteRightCounter = 0;
        int spriteDownCounter = 0;
        int spriteUpCounter = 0;
        //orientation of the player
        const byte None = 0, North = 1, East = 2, South = 3, West = 4;
        //bullet alive
        public bool bulletAlive;
        //bullet position
        public Vector2 bulletPosition;
        //bullet speed
        public int bulletMoveSpeed = 4;
        //player speed
        public int speed = 2;
        bool collision = false;

        public int ls1;
        public int ls2;
        public int ls3;
        public int ls4;
        public int ls5;
        public int ps1;

        public string[] LeaderBoard;
        
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int HostagesSaved
        {
            get { return hostagesSaved; }
            set { hostagesSaved = value; }
        }

        public int HostagesShot
        {
            get { return hostagesShot; }
            set { hostagesShot = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }
        public Player(Texture2D playerLeft1Texture, Texture2D playerLeft2Texture, Texture2D playerLeft3Texture, Texture2D playerLeft4Texture, Texture2D playerRight1Texture, Texture2D playerRight2Texture, Texture2D playerRight3Texture, Texture2D playerRight4Texture, Texture2D playerFront1Texture, Texture2D playerFront2Texture, Texture2D playerFront3Texture, Texture2D playerBulletTexture, Texture2D playerBack1Texture, Texture2D playerBack2Texture, Texture2D playerBack3Texture)
        {
            //the textures for the player
            playerLeft1 = playerLeft1Texture;
            playerLeft2 = playerLeft2Texture;
            playerLeft3 = playerLeft3Texture;
            playerLeft4 = playerLeft4Texture;
            playerRight1 = playerRight1Texture;
            playerRight2 = playerRight2Texture;
            playerRight3 = playerRight3Texture;
            playerRight4 = playerRight4Texture;
            playerFront1 = playerFront1Texture;
            playerFront2 = playerFront2Texture;
            playerFront3 = playerFront3Texture;
            playerBullet = playerBulletTexture;
            playerBack1 = playerBack1Texture;
            playerBack2 = playerBack2Texture;
            playerBack3 = playerBack3Texture;
            //x and y co-ordinates
            playerPos.X = 40;
            playerPos.Y = 40;
            //bullet
            bulletAlive = false;
            bulletPosition.X = playerPos.X;
            bulletPosition.Y = playerPos.Y + 10;
            //sets starting info
            score = 50;
            lives = 3;
            health = 100;
            hostagesSaved = 0;
            hostagesShot = 0;
        }
        //move method, sprites change based on timer, face in whatever direction the player is moving in
        public void Move(int orientation)
        {
            if (orientation == East)
            {
                if (playerPos.X > 0)
                    playerPos.X = playerPos.X - speed;
                if (playerPos.X < 0)
                    playerPos.X = playerPos.X - 0;
                spriteLeftCounter++;
                if (spriteLeftCounter == 10)
                    spriteLeftCounter = 0;
            }
            else if (orientation == West)
            {
                if (playerPos.X < 770)
                    playerPos.X = playerPos.X + speed;
                if (playerPos.X > 770)
                    playerPos.X = playerPos.X + 0;
                spriteRightCounter++;
                if (spriteRightCounter == 10)
                    spriteRightCounter = 0;
            }
            else if (orientation == South)
            {
                playerPos.Y = playerPos.Y + speed;
                if (playerPos.Y >= 350)
                    playerPos.Y = 10;
                spriteDownCounter++;
                if (spriteDownCounter == 10)
                    spriteDownCounter = 0;
            }
            else if (orientation == North)
            {
                playerPos.Y = playerPos.Y - speed;
                spriteUpCounter++;
                if (spriteUpCounter == 10)
                    spriteUpCounter = 0;
            }

        }
        //collision with turret
        public void CollisionTurret(int turret1X, int turret1Y, int turretWidth, int turretHeight)//, int turret2X, int turret2Y)
        {
            if (turret1X + turretWidth <= playerPos.X)
            {
                collision = false;
            }
            else if (turret1X >= playerPos.X + 10)
            {
                collision = false;
            }
            else if (turret1Y + turretHeight <= playerPos.Y)
            {
                collision = false;
            }
            else if (turret1Y >= playerPos.Y + 10)
            {
                collision = false;
            }
            else collision = true;
            if (collision == true)
            {
                health -= 10;
                playerPos.X = 20;
            }
        }
        //attempt at leaderboard
        //save data to leaderboard
        public void WriteToLeaderBoardTextFile(StreamWriter outfile)
        {
            outfile.Write(name + (","));
            outfile.Write(score + (","));
        }
        //load data from leaderboard
        public void LoadFromLeaderBoardTextFile(string aline)
        {
            string[] wordArray;
            wordArray = aline.Split(',');
            string[] leaderArray;
            leaderArray = aline.Split(',');

            LeaderBoard[0] = "AAA,0";
            LeaderBoard[1] = "BBB,0";
            LeaderBoard[2] = "CCC,0";
            LeaderBoard[3] = "DDD,0";
            LeaderBoard[4] = "EEE,0";

            ls1 = Convert.ToInt32(leaderArray[1]);
            ls2 = Convert.ToInt32(leaderArray[3]);
            ls3 = Convert.ToInt32(leaderArray[5]);
            ls4 = Convert.ToInt32(leaderArray[7]);
            ls5 = Convert.ToInt32(leaderArray[9]);
            ps1 = Convert.ToInt32(wordArray[1]);
            if (ps1 > ls1)
            {
                LeaderBoard[0] = wordArray[0];
            }
            
            //name = Convert.ToString(wordArray[0]);
            //score = Convert.ToInt32(wordArray[1]);
            //LeaderBoard[0] = Convert.ToString(wordArray[0]);
            //LeaderBoard[0] = Convert.ToString(wordArray[1]);
        }
        //save data
        public void WriteToTextFile(StreamWriter outfile)
        {
            outfile.Write(score + (","));
            outfile.Write(name + (","));
            outfile.Write(hostagesSaved + (","));
            outfile.Write(lives + (","));
            outfile.Write(playerPos.X + (","));
            outfile.Write(playerPos.Y + (","));
        }
        //loads data
        public void LoadFromTextFile(string aLine)
        {
            string[] wordArray;
            wordArray = aLine.Split(',');
            score = Convert.ToInt32(wordArray[0]);
            name = Convert.ToString(wordArray[1]);
            hostagesSaved = Convert.ToInt32(wordArray[2]);
            lives = Convert.ToInt32(wordArray[3]);
            playerPos.X = Convert.ToInt32(wordArray[4]);
            playerPos.Y = Convert.ToInt32(wordArray[5]);
        }
        public void Draw(SpriteBatch spriteBatch, int orientation, SpriteFont font)
        {
            //if facing Left draw these sprites
            if (orientation == East && spriteLeftCounter <= 4)
                spriteBatch.Draw(playerLeft1, new Rectangle((int)playerPos.X,(int)playerPos.Y,playerLeft1.Width,playerLeft1.Height), Color.White);
            else if (orientation == East && spriteLeftCounter <= 6)
                spriteBatch.Draw(playerLeft2, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerLeft1.Width, playerLeft1.Height), Color.White);
            else if (orientation == East && spriteLeftCounter <= 8)
                spriteBatch.Draw(playerLeft3, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerLeft1.Width, playerLeft1.Height), Color.White);
            else if (orientation == East && spriteLeftCounter <= 10)
                spriteBatch.Draw(playerLeft4, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerLeft1.Width, playerLeft1.Height), Color.White);

            //if facing Right draw these sprites
            if (orientation == West && spriteRightCounter <= 4)
                spriteBatch.Draw(playerRight1, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerRight1.Width, playerRight1.Height), Color.White);
            else if (orientation == West && spriteRightCounter <= 6)
                spriteBatch.Draw(playerRight2, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerRight1.Width, playerRight1.Height), Color.White);
            else if (orientation == West && spriteRightCounter <= 8)
                spriteBatch.Draw(playerRight3, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerRight1.Width, playerRight1.Height), Color.White);
            else if (orientation == West && spriteRightCounter <= 10)
                spriteBatch.Draw(playerRight4, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerRight1.Width, playerRight1.Height), Color.White);

            //if facing down draw these sprites
            if (orientation == South && spriteDownCounter <= 4)
                spriteBatch.Draw(playerFront3, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerFront1.Width, playerFront1.Height), Color.White);
            else if (orientation == South && spriteDownCounter <= 6)
                spriteBatch.Draw(playerFront1, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerFront1.Width, playerFront1.Height), Color.White);
            else if (orientation == South && spriteDownCounter <= 8)
                spriteBatch.Draw(playerFront2, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerFront1.Width, playerFront1.Height), Color.White);
            else if (orientation == South && spriteDownCounter <= 10)
                spriteBatch.Draw(playerFront3, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerFront1.Width, playerFront1.Height), Color.White);

            //if facing up draw these sprites
            if (orientation == North && spriteUpCounter <= 4)
                spriteBatch.Draw(playerBack1, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerBack1.Width, playerBack1.Height), Color.White);
            else if (orientation == North && spriteUpCounter <= 6)
                spriteBatch.Draw(playerBack2, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerBack1.Width, playerBack1.Height), Color.White);
            else if (orientation == North && spriteUpCounter <= 8)
                spriteBatch.Draw(playerBack3, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerBack1.Width, playerBack1.Height), Color.White);
            else if (orientation == North && spriteUpCounter <= 10)
                spriteBatch.Draw(playerBack1, new Rectangle((int)playerPos.X, (int)playerPos.Y, playerBack1.Width, playerBack1.Height), Color.White);

            spriteBatch.DrawString(font, "Score : " + score, new Vector2(20, 380), Color.White);
            spriteBatch.DrawString(font, "Name : " + name, new Vector2(20, 400), Color.White);
            spriteBatch.DrawString(font, "Lives : " + lives, new Vector2(200, 380), Color.White);

            if (lives > 0)
                spriteBatch.Draw(playerRight1, new Rectangle(200, 410, 20, 20), Color.White);
            if (lives > 1)
                spriteBatch.Draw(playerRight1, new Rectangle(220, 410, 20, 20), Color.White);
            if (lives > 2)
                spriteBatch.Draw(playerRight1, new Rectangle(240, 410, 20, 20), Color.White);

            spriteBatch.DrawString(font, "Health : " + health, new Vector2(380, 380), Color.White);
            spriteBatch.DrawString(font, "Hostages saved : " + hostagesSaved, new Vector2(380, 400), Color.White);
            spriteBatch.DrawString(font, "Hostages lost : " + hostagesShot, new Vector2(580, 380), Color.White);
        }
    }
}
