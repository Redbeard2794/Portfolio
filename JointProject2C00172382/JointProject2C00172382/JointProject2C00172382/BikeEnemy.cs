using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace JointProject2C00172382
{//super class
    public class BikeEnemy
    {
        //textures
        Texture2D bikeTexture1;
        Texture2D wheelieTexture;
        Texture2D bikeTexture2;
        Texture2D bikeRightWheelie;
        Texture2D bikeUp;
        Texture2D bikeDown;
        //bike position
        public Vector2 bikePos;
        //speed
        public int speed = 2;
        //whether bike is in a wheelie or not
        bool wheelie = false;
        int distanceX = 0;
        int distanceY = 0;
        //if facing left
        public bool left = true;
        //if facing right
        public bool right = false;
        //if alive
        public bool alive = true;
        public BikeEnemy()
        {
            //sets position and alive
            bikePos.X = 300;
            bikePos.Y = 100;
            alive = true;
        }
        public BikeEnemy(Texture2D bikeLeft, Texture2D bikeRight, Texture2D wheelie1, Texture2D wheelieRight, Texture2D up, Texture2D down)
        {
            //textures
            bikeTexture1 = bikeLeft;
            bikeTexture2 = bikeRight;
            wheelieTexture = wheelie1;
            bikeRightWheelie = wheelieRight;
            bikeUp = up;
            bikeDown = down;
        }
        //move method
        public virtual void Move(int playerX, int playerY)
        {
            //if facing left
                if (left == true)
                {
                    bikePos.X -= speed;
                }
                    //if facing right
                else if (right == true)
                {
                    bikePos.X += speed;
                }
                if (playerY < bikePos.Y)
                    bikePos.Y -= speed;
                else if (playerY > bikePos.Y)
                    bikePos.Y += speed;
            //changes direction
                if (playerX > bikePos.X)
                {
                    left = false;
                    right = true;
                }
                else if (playerX < bikePos.X)
                {
                    right = false;
                    left = true;
                }
        }
        //rectangle collision with player
        public virtual bool CollisionWithPlayer(int playerX, int playerY, int playerWidth, int playerHeight)
        {
            if (alive == true)
            {
                if (playerX >= bikePos.X + bikeTexture1.Width)
                {
                    return false;
                }
                if (playerY + playerHeight <= bikePos.Y)
                {
                    return false;
                }
                if (playerX + playerWidth <= bikePos.X)
                {
                    return false;
                }
                if (playerY >= bikePos.Y + bikeTexture1.Height)
                {
                    return false;
                }
                return true;
            }
            else return false;
        }
        //changes to a wheelie sprite if the player is close enough
        public void Wheelie(int playerX, int playerY)
        {
            distanceX = playerX - (int)bikePos.X;
            distanceY = playerY - (int)bikePos.Y;

            if (distanceX <= 50)
                wheelie = true;
            if (distanceX >= -50)
                wheelie = true;

            else wheelie = false;
        }
        //save data
        public void WriteToTextFile(StreamWriter outfile)
        {
            outfile.Write(alive + (","));
            outfile.Write(bikePos.X + (","));
            outfile.Write(bikePos.Y + (","));
        }
        //loads data
        public void LoadFromTextFile(string bLine)
        {
            string[] wordArray;
            wordArray = bLine.Split(',');
            alive = Convert.ToBoolean(wordArray[0]);
            bikePos.X = Convert.ToInt32(wordArray[1]);
            bikePos.Y = Convert.ToInt32(wordArray[2]);
        }
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (alive == true)
            {
                if (left == true)
                    spriteBatch.Draw(bikeTexture1, new Rectangle((int)bikePos.X, (int)bikePos.Y, bikeTexture2.Width, bikeTexture2.Height), Color.White);
                else if (right == true)
                    spriteBatch.Draw(bikeTexture2, new Rectangle((int)bikePos.X, (int)bikePos.Y, bikeTexture2.Width, bikeTexture2.Height), Color.White);
                if (wheelie == true && left == true)
                    spriteBatch.Draw(wheelieTexture, new Rectangle((int)bikePos.X, (int)bikePos.Y, bikeTexture2.Width, bikeTexture2.Height), Color.White);
                else if (wheelie == true && right == true)
                    spriteBatch.Draw(bikeRightWheelie, new Rectangle((int)bikePos.X, (int)bikePos.Y, bikeTexture2.Width, bikeTexture2.Height), Color.White);
            }
        }
    }
}
