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
    class Hostage
    {
        //textures
        Texture2D hostageUp1;
        Texture2D hostageUp2;
        Texture2D hostageUp3;
        Texture2D hostageUp4;
        Texture2D hostageUp5;
        Texture2D hostageUp6;
        Texture2D hostageBack1;
        Texture2D hostageBack2;
        Texture2D hostageBack3;
        Texture2D hostageBack4;
        Texture2D hostageBack5;
        Texture2D hostageBack6;
        //position of hostage
        Vector2 hostagePos;
        //direction hostage is moving in
        bool moveUp = true;
        bool moveDown = false;
        //counters for changing between sprites
        int spriteCountUp = 0;
        int spriteCountDown = 0;
        public Hostage(Texture2D up1, Texture2D up2, Texture2D up3, Texture2D up4, Texture2D up5, Texture2D up6, Texture2D back1, Texture2D back2, Texture2D back3, Texture2D back4, Texture2D back5, Texture2D back6)
        {
            //saves the textures
            hostageUp1 = up1;
            hostageUp2 = up2;
            hostageUp3 = up3;
            hostageUp4 = up4;
            hostageUp5 = up5;
            hostageUp6 = up6;
            hostageBack1 = back1;
            hostageBack2 = back2;
            hostageBack3 = back3;
            hostageBack4 = back4;
            hostageBack5 = back5;
            hostageBack6 = back6;
            //sets x and y position
            hostagePos.X = 600;
            hostagePos.Y = 250;
        }
        //move method
        public void Move()
        {
            //if going down y + 1. sprite down counter increases. when it hits 12 set to zero
            if (moveDown == true)
            {
                hostagePos.Y += 1;
                spriteCountDown++;
                if (spriteCountDown == 12)
                    spriteCountDown = 0;
            }
            //if going up y - 1. sprite up counter increases. when it hits 12 set to zero
            else if (moveUp == true)
            {
                hostagePos.Y -= 1;
                spriteCountUp++;
                if (spriteCountUp == 12)
                    spriteCountUp = 0;
            }
            //changes direction from down to up at y = 300
            if (hostagePos.Y > 300)
            {
                moveDown = false;
                moveUp = true;
            }
                //changes direction from up to down at y =150
            else if (hostagePos.Y < 150)
            {
                moveUp = false;
                moveDown = true;
            }
        }
        //detects rectangle collision between layer and hostage
        public bool Collision(int playerX, int playerY, int playerWidth, int playerHeight)
        {
                if (playerX >= hostagePos.X + hostageBack1.Width)
                {
                    return false;
                }
                if (playerY + playerHeight <= hostagePos.Y)
                {
                    return false;
                }
                if (playerX + playerWidth <= hostagePos.X)
                {
                    return false;
                }
                if (playerY >= hostagePos.Y + hostageBack1.Height)
                {
                    return false;
                }
                return true;
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //if moving up draws these sprites depending on counter
            if (moveUp == true)
            {
                if (spriteCountUp <= 2)
                    spriteBatch.Draw(hostageBack1, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountUp <= 4)
                    spriteBatch.Draw(hostageBack2, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountUp <= 6)
                    spriteBatch.Draw(hostageBack3, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountUp <= 8)
                    spriteBatch.Draw(hostageBack4, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountUp <= 10)
                    spriteBatch.Draw(hostageBack5, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountUp <= 12)
                    spriteBatch.Draw(hostageBack6, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
            }
            //if moving down draws these sprites depending on counter
            else if (moveDown == true)
            {
                if (spriteCountDown <= 2)
                    spriteBatch.Draw(hostageUp1, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountDown <= 4)
                    spriteBatch.Draw(hostageUp2, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountDown <= 6)
                    spriteBatch.Draw(hostageUp3, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountDown <= 8)
                    spriteBatch.Draw(hostageUp4, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountDown <= 10)
                    spriteBatch.Draw(hostageUp5, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
                else if (spriteCountDown <= 12)
                    spriteBatch.Draw(hostageUp6, new Rectangle((int)hostagePos.X, (int)hostagePos.Y, hostageUp1.Width, hostageUp1.Height), Color.White);
            }

        }
    }
}
